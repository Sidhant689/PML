using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Pml.Shared.DTOs.Master.SystemAdminDTOs;
using PMLERP.IServices.Master;

namespace PMLERP.Services.Master
{
    public class SystemAdminService : ISystemAdminUserService
    {
        private readonly HttpClient _httpClient;
        public SystemAdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SystemAdminUserDto> CreateAdminAsync(SystemAdminUserDto adminDto)
        {
            try
            {
                // Map DTO to entity and hash the password
                var admin = new SystemAdminUserDto
                {
                    Name = adminDto.Name,
                    UserName = adminDto.UserName,
                    Password = BCrypt.Net.BCrypt.HashPassword(adminDto.Password), // hash it
                    UserStatus = adminDto.UserStatus,
                    UserEmail = adminDto.UserEmail,
                    UserPhone = adminDto.UserPhone,
                    UserAddress = adminDto.UserAddress
                };
                // Call API to create admin
                var response = await _httpClient.PostAsJsonAsync("SystemAdmin/create", admin);
                response.EnsureSuccessStatusCode();
                // Read the created admin from response
                var createdAdmin = await response.Content.ReadFromJsonAsync<SystemAdminUserDto>();
                // Map entity to response DTO
                return new SystemAdminUserDto
                {
                    Id = createdAdmin.Id,
                    Name = createdAdmin.Name,
                    UserName = createdAdmin.UserName,
                    UserStatus = createdAdmin.UserStatus,
                    UserEmail = createdAdmin.UserEmail,
                    UserPhone = createdAdmin.UserPhone,
                    UserAddress = createdAdmin.UserAddress
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating admin: {ex.Message}");
                throw;
            }
        }

        public async Task<SystemAdminUserDto> UpdateAdminAsync(SystemAdminUserDto adminDto)
        {
            try
            {
                // Map DTO to entity and hash the password if it has changed
                var admin = new SystemAdminUserDto
                {
                    Id = adminDto.Id,
                    Name = adminDto.Name,
                    UserName = adminDto.UserName,
                    Password = BCrypt.Net.BCrypt.HashPassword(adminDto.Password), // hash it
                    UserStatus = adminDto.UserStatus,
                    UserEmail = adminDto.UserEmail,
                    UserPhone = adminDto.UserPhone,
                    UserAddress = adminDto.UserAddress
                };
                // Call API to update admin
                var response = await _httpClient.PutAsJsonAsync($"SystemAdmin/update/{admin.Id}", admin);
                response.EnsureSuccessStatusCode();
                // Read the updated admin from response
                var updatedAdmin = await response.Content.ReadFromJsonAsync<SystemAdminUserDto>();
                return updatedAdmin;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating admin: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            try
            {
                // Call API to delete admin
                var response = await _httpClient.DeleteAsync($"SystemAdmin/delete/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting admin: {ex.Message}");
                throw;
            }
        }

        public async Task<SystemAdminUserDto> GetAdminByIdAsync(int id)
        {
            try
            {
                // Call API to get admin by ID
                var response = await _httpClient.GetAsync($"SystemAdmin/get/{id}");
                response.EnsureSuccessStatusCode();
                // Read the admin from response
                return await response.Content.ReadFromJsonAsync<SystemAdminUserDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting admin by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<List<SystemAdminUserDto>> GetAllAdminsAsync()
        {
            try
            {
                // Call API to get all admins
                var response = await _httpClient.GetAsync("SystemAdmin/getAll");
                response.EnsureSuccessStatusCode();
                // Read the list of admins from response
                return await response.Content.ReadFromJsonAsync<List<SystemAdminUserDto>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all admins: {ex.Message}");
                throw;
            }
        }
    }
}
