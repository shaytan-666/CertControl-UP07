using System;

namespace CertControl.Dtos
{
    public class CertificateDto
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime EndDate { get; set; }

        public string StatusName { get; set; }
        public string TypeName { get; set; }
        public string AuthorityName { get; set; }
        public string OwnerName { get; set; }

        public bool IsExpired => DateTime.Now > EndDate;

        public string ServerNames { get; set; }
    }
}
