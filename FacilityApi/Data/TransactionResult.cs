using System;
namespace FacilityApi.Data
{
	public enum TransactionResult
	{
		Success,
		BadRequest,
		Conflict,
		NotFound,
		ServerError
	}
}

