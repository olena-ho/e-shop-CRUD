using Eshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.RepositoryInterfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Customer>> GetAllAsync(CancellationToken ct = default);
    Task<Customer> AddAsync(Customer entity, CancellationToken ct = default);
    Task UpdateAsync(Customer entity, CancellationToken ct = default);
    Task DeleteAsync(Customer entity, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, int? exceptId = null, CancellationToken ct = default);
}
