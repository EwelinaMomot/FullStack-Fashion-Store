namespace server.DTOs
{
    public class UpdateUserRoleDto
    {
        public required int UserId { get; set; }
        public required int RoleId {get;set;}
    }
}
