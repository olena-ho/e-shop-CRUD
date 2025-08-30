namespace Eshop.Application.Customers;

public interface ICustomerService
{
    Task<List<CustomerResponse>> GetAllAsync(CancellationToken ct = default);
    Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CustomerResponse> CreateAsync(CustomerRequestDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, CustomerRequestDto dto, CancellationToken ct = default);
    Task<bool> PatchPhoneAsync(int id, string? phone, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
