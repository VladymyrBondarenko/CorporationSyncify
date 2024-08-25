﻿namespace CorporationSyncify.HRS.Infrastructure.Kafka
{
    public class KafkaMessage
    {
        public Guid EventId { get; set; }

        public DateTimeOffset EmittedAt { get; set; }

        public string? ContentBlob { get; set; }
    }
}
