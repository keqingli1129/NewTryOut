using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FBISDBlazorServerApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await httpClient.GetFromJsonAsync<Employee[]>("api/Employees");
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await httpClient.GetFromJsonAsync<Employee>($"api/Employees/{id}");
        }

        public async Task<Employee> UpdateEmployee(Employee updatedEmployee)
        {
            int id = updatedEmployee.EmployeeId;
            HttpResponseMessage response = await httpClient.PutAsJsonAsync<Employee>($"api/employees/{id}", updatedEmployee);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Employee>();
            }
            return null;
        }

        public async Task<Employee> CreateEmployee(Employee newEmployee)
        {
            int id = newEmployee.EmployeeId;
            HttpResponseMessage response = await httpClient.PostAsJsonAsync<Employee>("api/Employees", newEmployee);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Employee>();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
            }
            return null;
        }
        public async Task DeleteEmployee(int id)
        {
            await httpClient.DeleteAsync($"api/employees/{id}");
        }
    }
}
