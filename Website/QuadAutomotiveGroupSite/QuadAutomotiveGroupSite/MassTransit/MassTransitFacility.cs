﻿// Copyright (c)  2012 QuadAutomotive Group.
// All rights reserved.
// 
// Redistribution and use in source and binary forms are permitted
// provided that the above copyright notice and this paragraph are
// duplicated in all such forms and that any documentation,
// advertising materials, and other materials related to such
// distribution and use acknowledge that the software was developed
// by the <organization>.  The name of the
// University may not be used to endorse or promote products derived
// from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED ``AS IS'' AND WITHOUT ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.

using System;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using QuadAutomotiveGroupSite.Services;

namespace QuadAutomotiveGroupSite.MassTransit
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            container.Register(Component.For<IPropertyService>().ImplementedBy<DotNetPropertyService>());
            var properties = container.Resolve<IPropertyService>();
            container.AddFacility<MassTransitFacility>(
                f => f.ReceiveFromUri = properties.Get<string>("MassTransitRecieveFrom"));
        }
    }

    public class MassTransitFacility : AbstractFacility
    {
        public virtual string ReceiveFromUri { get; set; }

        protected virtual IServiceBus ConfigureServiceBus()
        {
            var container = Kernel.Resolve<IWindsorContainer>();

            var bus = ServiceBusFactory.New(
                sbc =>
                    {
                        sbc.UseRabbitMq();
                        sbc.UseRabbitMqRouting();
                        sbc.ReceiveFrom(ReceiveFromUri);
                        sbc.UseControlBus();
                        sbc.Subscribe(c => c.LoadFrom(container));
                    });

            return bus;
        }

        protected override void Init()
        {
            var filter = new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory, "QuadAuto.Server.*.dll");

            var x =
                AllTypes.FromAssemblyInDirectory(filter).BasedOn<IConsumer>().WithServiceSelect(
                    (t, b) => new[] {t, typeof (IConsumer)});

            Kernel.Register(x);

            var y =
                AllTypes.FromThisAssembly().BasedOn<IConsumer>().WithServiceSelect(
                    (t, b) => new[] {t, typeof (IConsumer)});

            Kernel.Register(y);

            Kernel.Register(Component.For<IServiceBus>().UsingFactoryMethod(ConfigureServiceBus));
        }
    }
}