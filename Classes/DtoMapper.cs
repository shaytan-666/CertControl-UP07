using System.Linq;
using CertControl.Data;
using CertControl.Dtos;

namespace CertControl.Classes
{
    public static class DtoMapper
    {
        public static CertificateDto GetCertificateDto(Certificate certificate) =>
            new CertificateDto
            {
                Id = certificate.id,
                SerialNumber = certificate.serial_number,
                Name = certificate.name,
                IssueDate = certificate.issue_date,
                EndDate = certificate.end_date,
                StatusName = certificate.CertificateStatus.status,
                TypeName = certificate.CertificateType.type,
                AuthorityName = certificate.CertificateAuthority.name,
                OwnerName = certificate.Owner.full_name,
                ServerNames = string.Join(", ", certificate.Server.Select(s => s.ip).ToList())
            };
    }
}
