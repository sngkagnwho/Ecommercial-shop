using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Identity_Auth
{
    public class RefreshToken : BaseEntity
    {
        public int UserId { get; private set; }
        public virtual User? User { get; set; }
        
        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? RevokedByIp { get; private set; }
        public string? RevokedReason { get; private set; }
        
        // Token Rotation - để detect token reuse attack
        public string? ReplacedByToken { get; private set; }
        
        public string? DeviceInfo { get; private set; }
        public string? IpAddress { get; private set; }
        
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsActive => !IsRevoked && !IsExpired;
        
        protected RefreshToken() { }
        
        public RefreshToken(int userId, string token, DateTime expiresAt, string? deviceInfo, string? ipAddress)
        {
            UserId = userId;
            Token = token ?? throw new ArgumentNullException(nameof(token));
            ExpiresAt = expiresAt;
            DeviceInfo = deviceInfo;
            IpAddress = ipAddress;
        }
        
        public void Revoke(string? ipAddress, string? reason = null)
        {
            RevokedAt = DateTime.UtcNow;
            RevokedByIp = ipAddress;
            RevokedReason = reason;
        }
        
        public void ReplaceWith(string newToken)
        {
            ReplacedByToken = newToken;
            Revoke(IpAddress, "Replaced by new token");
        }
    }
}
