using System;
using DataStore.Core.Models.Interfaces;
using System.Text.Json;

namespace DataStore.Core.Models
{
    public class CommunicationMessage : Meta
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SentByUserId { get; set; }
        public ApplicationUser SentByUser { get; set; }
        public DateTime SentDate { get; set; }
        public string Status { get; set; }
        public bool SentToAllUsers { get; set; }
        public string TargetedRolesJson { get; set; }
        public string TargetedDepartmentsJson { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public List<string> GetTargetedRoles()
        {
            return string.IsNullOrEmpty(TargetedRolesJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(TargetedRolesJson);
        }

        public void SetTargetedRoles(List<string> roles)
        {
            TargetedRolesJson = JsonSerializer.Serialize(roles);
        }

        public List<string> GetTargetedDepartments()
        {
            return string.IsNullOrEmpty(TargetedDepartmentsJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(TargetedDepartmentsJson);
        }

        public void SetTargetedDepartments(List<string> departments)
        {
            TargetedDepartmentsJson = JsonSerializer.Serialize(departments);
        }
    }
}