namespace NT.WEB.Authorization
{
    /// <summary>
    /// Định nghĩa thông tin của một Endpoint (Controller + Action) trong hệ thống.
    /// Dùng để tự động quét và đăng ký Permission.
    /// </summary>
    public class EndpointInfo
    {
        /// <summary>
        /// Tên Controller (không có hậu tố "Controller")
        /// </summary>
        public string Controller { get; set; } = string.Empty;

        /// <summary>
        /// Tên Action
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// HTTP Method (GET, POST, PUT, DELETE)
        /// </summary>
        public string HttpMethod { get; set; } = "GET";

        /// <summary>
        /// Route pattern
        /// </summary>
        public string Route { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả action
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Thuộc area admin hay client
        /// </summary>
        public bool IsAdminArea { get; set; }

        /// <summary>
        /// Mã quyền được tạo tự động: {Controller}.{Action}
        /// </summary>
        public string PermissionCode => $"{Controller}.{Action}";
    }
}
