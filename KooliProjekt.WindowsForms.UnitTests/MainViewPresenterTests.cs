using KooliProjekt.WindowsForms.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests
{
    public class MainViewPresenterTests
    {
        private readonly Mock<IClientsApiClient> _apiClientMock;
        private readonly Mock<IMainView> _mainViewMock;
        private readonly MainViewPresenter _presenter;

        public MainViewPresenterTests()
        {
            _apiClientMock = new Mock<IClientsApiClient>();
            _mainViewMock = new Mock<IMainView>();
            _presenter = new MainViewPresenter(_apiClientMock.Object, _mainViewMock.Object);
        }

        [Fact]
        public async Task LoadData_should_call_ShowError_with_faulty_response()
        {
            var faultyResponse = new OperationResult<PagedResult<Client>>();
            faultyResponse.AddError("An error occurred while fetching data.");

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = null)
                .Verifiable();

            await _presenter.LoadData();

            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task LoadData_should_set_DataSource_with_valid_response()
        {
            var response = new OperationResult<PagedResult<Client>>
            {
                Value = new PagedResult<Client>
                {
                    Results = new List<Client>
                    {
                        new Client { Id = 1, Name = "N1", Email = "n1@test.ee", Phone = "123", Address = "A1", Discount = 0.1m },
                        new Client { Id = 2, Name = "N2", Email = "n2@test.ee", Phone = "456", Address = "A2", Discount = 0.2m }
                    }
                }
            };

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(response)
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = response.Value.Results)
                .Verifiable();

            await _presenter.LoadData();

            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_clear_fields_with_null_selection()
        {
            _mainViewMock.SetupSet(view => view.SelectedItem = null).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentId = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentName = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentEmail = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentPhone = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentAddress = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentDiscount = string.Empty).Verifiable();

            _presenter.SetSelection(null);

            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_set_fields_with_valid_selection()
        {
            var selectedClient = new Client
            {
                Id = 7,
                Name = "Mati",
                Email = "mati@test.ee",
                Phone = "555",
                Address = "Tartu",
                Discount = 0.3m
            };

            _mainViewMock.SetupSet(view => view.SelectedItem = selectedClient).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentId = selectedClient.Id).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentName = selectedClient.Name).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentEmail = selectedClient.Email).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentPhone = selectedClient.Phone).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentAddress = selectedClient.Address).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentDiscount = selectedClient.Discount.ToString()).Verifiable();

            _presenter.SetSelection(selectedClient);

            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_ShowError_when_discount_is_not_number()
        {
            _mainViewMock.SetupGet(view => view.CurrentDiscount).Returns("not-a-number");
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            await _presenter.Save();

            _apiClientMock.Verify(client => client.Save(It.IsAny<Client>()), Times.Never);
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_ShowError_with_faulty_response()
        {
            var faultyResponse = new OperationResult();
            faultyResponse.AddError("An error occurred while saving data.");

            _mainViewMock.SetupGet(view => view.CurrentId).Returns(10);
            _mainViewMock.SetupGet(view => view.CurrentName).Returns("Name");
            _mainViewMock.SetupGet(view => view.CurrentEmail).Returns("name@test.ee");
            _mainViewMock.SetupGet(view => view.CurrentPhone).Returns("123");
            _mainViewMock.SetupGet(view => view.CurrentAddress).Returns("Addr");
            _mainViewMock.SetupGet(view => view.CurrentDiscount).Returns("0.15");

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<Client>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            await _presenter.Save();

            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_LoadData_with_valid_response()
        {
            var listResponse = new OperationResult<PagedResult<Client>>
            {
                Value = new PagedResult<Client> { Results = new List<Client>() }
            };

            _mainViewMock.SetupGet(view => view.CurrentId).Returns(1);
            _mainViewMock.SetupGet(view => view.CurrentName).Returns("Client");
            _mainViewMock.SetupGet(view => view.CurrentEmail).Returns("client@test.ee");
            _mainViewMock.SetupGet(view => view.CurrentPhone).Returns("123");
            _mainViewMock.SetupGet(view => view.CurrentAddress).Returns("Address");
            _mainViewMock.SetupGet(view => view.CurrentDiscount).Returns("0.2");

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<Client>()))
                .ReturnsAsync(new OperationResult())
                .Verifiable();
            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(listResponse)
                .Verifiable();
            _mainViewMock.SetupSet(view => view.SelectedItem = null).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentId = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentName = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentEmail = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentPhone = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentAddress = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentDiscount = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.DataSource = listResponse.Value.Results).Verifiable();

            await _presenter.Save();

            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_when_no_selection()
        {
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            await _presenter.Delete();

            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
            _mainViewMock.Verify(view => view.ConfirmDelete(), Times.Never);
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_when_user_didnot_confirmed()
        {
            var selectedClient = new Client { Id = 5, Name = "ToDelete" };
            _presenter.SetSelection(selectedClient);

            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(false)
                .Verifiable();

            await _presenter.Delete();

            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_call_ShowError_with_faulty_response()
        {
            var selectedClient = new Client { Id = 6, Name = "ToDelete" };
            var faultyResponse = new OperationResult();
            faultyResponse.AddError("An error occurred while deleting data.");
            _presenter.SetSelection(selectedClient);

            _mainViewMock.Setup(view => view.ConfirmDelete()).Returns(true).Verifiable();
            _apiClientMock
                .Setup(client => client.Delete(selectedClient.Id))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            await _presenter.Delete();

            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_call_LoadData_with_valid_response()
        {
            var selectedClient = new Client { Id = 9, Name = "ToDelete" };
            var listResponse = new OperationResult<PagedResult<Client>>
            {
                Value = new PagedResult<Client> { Results = new List<Client>() }
            };
            _presenter.SetSelection(selectedClient);

            _mainViewMock.Setup(view => view.ConfirmDelete()).Returns(true).Verifiable();
            _apiClientMock
                .Setup(client => client.Delete(selectedClient.Id))
                .ReturnsAsync(new OperationResult())
                .Verifiable();
            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(listResponse)
                .Verifiable();
            _mainViewMock.SetupSet(view => view.SelectedItem = null).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentId = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentName = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentEmail = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentPhone = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentAddress = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentDiscount = string.Empty).Verifiable();
            _mainViewMock.SetupSet(view => view.DataSource = listResponse.Value.Results).Verifiable();

            await _presenter.Delete();

            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }
    }
}
