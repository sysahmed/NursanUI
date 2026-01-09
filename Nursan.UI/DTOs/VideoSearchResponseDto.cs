namespace Nursan.UI.DTOs
{
    /// <summary>
    /// DTO за response от видео API search endpoint
    /// </summary>
    public class VideoSearchResponseDto
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public List<VideoItemDto> Items { get; set; } = new List<VideoItemDto>();
    }

    /// <summary>
    /// DTO за видео item в search response
    /// </summary>
    public class VideoItemDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public string? UploadedBy { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsMobileOptimized { get; set; }
        public string? Url { get; set; }
    }
}
