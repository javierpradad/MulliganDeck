using Microsoft.EntityFrameworkCore;
using MulliganDeck.Domain;

namespace MulliganDeck.Infrastructure.Auth;

public class AuthService
{
    private readonly MulliganDeckContext _context;

    public AuthService(MulliganDeckContext context)
    {
        _context = context;
    }

    public async Task<User?> RegisterAsync(string email, string password)
    {
        // ¿Ya existe un usuario con ese email?
        var exists = await _context.Users.AnyAsync(u => u.Email == email);
        if (exists)
            return null;

        // Hashear la contraseña (NUNCA se guarda en claro)
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}