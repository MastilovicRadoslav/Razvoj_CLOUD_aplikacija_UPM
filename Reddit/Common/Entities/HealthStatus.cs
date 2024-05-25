using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Common.Entities
{
    public class HealthStatus : TableEntity
    {
        public string ServiceType { get; set; }
        public string Status { get; set; }

        private static long _redditCounter = 0;
        private static readonly object _redditLock = new object();

        private static long _notificationCounter = 0;
        private static readonly object _notificationLock = new object();

        public HealthStatus(string serviceType)
        {
            PartitionKey = "HealthStatus";
            RowKey = $"{serviceType}-{GenerateNextId(serviceType)}";
            ServiceType = serviceType;
        }

        public HealthStatus() { }

        public HealthStatus(string serviceType, string status)
        {
            PartitionKey = "HealthStatus";
            RowKey = $"{serviceType}-{GenerateNextId(serviceType)}";
            ServiceType = serviceType;
            Status = status;
        }

        private static long GenerateNextId(string serviceType)
        {
            if (serviceType.Equals("RedditService", StringComparison.OrdinalIgnoreCase))
            {
                lock (_redditLock)
                {
                    return _redditCounter++;
                }
            }
            else if (serviceType.Equals("NotificationService", StringComparison.OrdinalIgnoreCase))
            {
                lock (_notificationLock)
                {
                    return _notificationCounter++;
                }
            }
            else
            {
                throw new ArgumentException("Unknown service type");
            }
        }
    }
}