using System;
using System.Collections.Generic;

namespace Beacon.ViewModels
{
    public class HomeIndexViewModel
    {
        public IReadOnlyList<AlertItem> Alerts { get; init; } = Array.Empty<AlertItem>();
        public IReadOnlyList<UpdateItem> Updates { get; init; } = Array.Empty<UpdateItem>();

        public record AlertItem(
            string Id,
            string Type,
            string Content,
            string Location,
            string? ImageUrl,
            DateTime CreatedAt,
            string AdminDisplayName,
            string? AdminAvatarUrl
        );

        public record UpdateItem(
            string Id,
            string Type,
            string Content,
            string Location,
            string? ImageUrl,
            DateTime CreatedAt,
            string AdminDisplayName,
            string? AdminAvatarUrl
        );
    }
}
