﻿using MediatR;

namespace CorporationSyncify.Identity.WebApi.Events
{
    public interface IIdentityEvent : INotification
    {
        public string EventName { get; }
    }
}
