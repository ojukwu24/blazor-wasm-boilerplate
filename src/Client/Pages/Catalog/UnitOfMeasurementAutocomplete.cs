using FSH.BlazorWebAssembly.Client.Infrastructure.ApiClient;
using FSH.BlazorWebAssembly.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
namespace FSH.BlazorWebAssembly.Client.Pages.Catalog;

public class UnitOfMeasurementAutocomplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<UnitOfMeasurementAutocomplete> L { get; set; } = default!;
    [Inject]
    private IUnitsOfMeasurementClient UnitsOfMeasurementClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<UnitOfMeasurementDto> _unitOfMeasurements = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Default Unit of Measurement"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchUnitsOfMeasurements;
        ToStringFunc = GetUnitsOfMeasurementName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one brand to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => UnitsOfMeasurementClient.GetAsync(_value, null), Snackbar) is { } unitOfMeasurement)
        {
            _unitOfMeasurements.Add(unitOfMeasurement);
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchUnitsOfMeasurements(string value)
    {
        var filter = new SearchUnitOfMeasurementRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => UnitsOfMeasurementClient.SearchAsync(null, filter), Snackbar)
            is PaginationResponseOfUnitOfMeasurementDto response)
        {
            _unitOfMeasurements = response.Data.ToList();
        }

        return _unitOfMeasurements.Select(x => x.Id);
    }

    private string GetUnitsOfMeasurementName(Guid id) =>
        _unitOfMeasurements.Find(b => b.Id == id)?.Name ?? string.Empty;
}
