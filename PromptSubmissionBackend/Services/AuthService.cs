using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PromptSubmissionBackend.Data;
using PromptSubmissionBackend.DTOs;
using PromptSubmissionBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PromptSubmissionBackend.Services;

public class AuthService(ApplicationDbContext _db, IConfiguration _config) : IAuthService
{
    public async Task<AuthResult> CreateAdminAsync(AdminCreateDto dto)
    {
        if (await _db.Admins.AnyAsync(a => a.Email == dto.Email))
        {
            return new AuthResult(false, "Admin already exists.");
        }

        var admin = new Admin
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password,
            CreatedAt = DateTime.UtcNow
        };

        _db.Admins.Add(admin);
        await _db.SaveChangesAsync();

        return new AuthResult(true, "Admin created successfully.");
    }

    public async Task<AuthResult> SignupAsync(SignupRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
        {
            return new AuthResult { Success = false, Message = "Email already exists." };
        }

        if (!IsValidEmail(request.Email))
            return new AuthResult(false, "Invalid email format.");

        var user = new AppUser
        {
            Email = request.Email,
            Password = request.Password,
            Role = "user"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // No ID needed here (optional)
        var token = GenerateJwtToken(user.Email, user.Role);
        return new AuthResult { Success = true, Message = "Signup successful.", Token = token };
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request, string role)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return new AuthResult(false, "Missing credentials.");

        if (role == "admin")
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Email == request.Email);
            if (admin == null || admin.Password != request.Password)
                return new AuthResult(false, "Invalid admin credentials.");

            var token = GenerateJwtToken(admin.Email, "admin");
            return new AuthResult(true, "Admin login successful.", token);
        }
        else // user
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || user.Password != request.Password)
                return new AuthResult(false, "Invalid user credentials.");

            var token = GenerateJwtToken(user.Email, "user", user.Id);
            return new AuthResult(true, "User login successful.", token);
        }
    }

    // For users (with userId claim)
    private string GenerateJwtToken(string email, string role, int userId)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        return BuildToken(claims);
    }

    // For admins or when ID not required
    private string GenerateJwtToken(string email, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        return BuildToken(claims);
    }

    // Shared JWT creation logic
    private string BuildToken(IEnumerable<Claim> claims)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
