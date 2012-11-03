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

namespace QuadAuto.Web.Controllers.Committees.Commands
{
    public class AddNewCommitteeContentPage : BaseModel
    {
        public AddNewCommitteeContentPage(Guid id, Guid committeeId, string content, string menuTitle, string title)
            : base(id)
        {
            CommitteeId = committeeId;
            Content = content;
            MenuTitle = menuTitle;
            Title = title;
        }

        public Guid CommitteeId { get; set; }
        public string MenuTitle { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}