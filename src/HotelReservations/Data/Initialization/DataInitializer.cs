using HotelReservations.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace HotelReservations.Data.Initialization;

public class DataInitializer
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public DataInitializer(RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    
    public async Task Seed()
    {
        var userRole = await _roleManager.FindByNameAsync("User");
        var adminRole = await _roleManager.FindByNameAsync("Admin");
        var employeeRole = await _roleManager.FindByNameAsync("Employee");
        var admin = await _userManager.FindByNameAsync("admin");
        var user = await _userManager.FindByNameAsync("user");
        
        if (userRole is null)
        {
            await _roleManager.CreateAsync(new Role() {Name = "User"});
            userRole = await _roleManager.FindByNameAsync("User");
        }
        
        if (adminRole is null)
        {
            await _roleManager.CreateAsync(new Role() {Name = "Admin"});
            adminRole = await _roleManager.FindByNameAsync("Admin");
        }
        
        if (employeeRole is null)
        {
            await _roleManager.CreateAsync(new Role() {Name = "Employee"});
            employeeRole = await _roleManager.FindByNameAsync("Employee");
        }

        if (admin is null)
        {
            await _userManager.CreateAsync(new User() {UserName = "admin"}, "admin");
            admin = await _userManager.FindByNameAsync("admin");
            await _userManager.AddToRoleAsync(admin, "Admin");
        }
        
        if (user is null)
        {
            await _userManager.CreateAsync(new User() {UserName = "user"}, "user");
            user = await _userManager.FindByNameAsync("user");
            await _userManager.AddToRoleAsync(user, "User");
        }

        for (int i = 1; i <= 100; i++)
        {
            var name = "test" + i;
            var nameArray = name.ToCharArray();
            Array.Reverse(nameArray);
            var nameReversed = new string(nameArray);
            var password = "test" + i;
            var testUser = await _userManager.FindByNameAsync(name);
            if (testUser is null)
            {
                await _userManager.CreateAsync(new User()
                {
                    Email = name + "@" + name + "." + name, 
                    FirstName = name,
                    LastName = nameReversed,
                    IsAdult = i % 2 == 0,
                    PhoneNumber = (i * 10230102).ToString(),
                    UserName = name
                }, password);
            }
        }
    }
}