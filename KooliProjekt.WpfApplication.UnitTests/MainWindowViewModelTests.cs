using Moq;

namespace KooliProjekt.WpfApplication.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IClientsApiClient> _apiClientMock;
        private readonly Mock<IDialogProvider> _dialogProviderMock;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _apiClientMock = new Mock<IClientsApiClient>();
            _dialogProviderMock = new Mock<IDialogProvider>();
            _viewModel = new MainWindowViewModel(_apiClientMock.Object, _dialogProviderMock.Object);
        }

        [Fact]
        public void SelectedItem_should_return_correct_item()
        {
            // Arrange
            var item = new Client { Id = 1, Name = "Test" };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.Equal(item, _viewModel.SelectedItem);
        }

        [Fact]
        public void SelectedItem_should_call_notify_property_changed()
        {
            // Arrange
            var item = new Client { Id = 1, Name = "Test" };
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.SelectedItem))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public async Task LoadData_should_load_data_from_api_client()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Client>>
            {
                Value = new PagedResult<Client>
                {
                    Results = new List<Client>
                    {
                        new Client { Id = 1, Name = "Test 1" },
                        new Client { Id = 2, Name = "Test 2" }
                    }
                }
            };

            _apiClientMock.Setup(client => client.List(1, 10))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act
            await _viewModel.LoadDataAsync();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Equal(2, _viewModel.Data.Count);
            Assert.Equal(1, _viewModel.Data[0].Id);
            Assert.Equal(2, _viewModel.Data[1].Id);
        }

        [Fact]
        public async Task LoadData_should_show_error_when_api_client_fails()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Client>>
            {
                Errors = new List<string> { "Error" }
            };

            _apiClientMock.Setup(client => client.List(1, 10))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act
            await _viewModel.LoadDataAsync();

            // Assert
            _apiClientMock.VerifyAll();
            _dialogProviderMock.Verify(dialog => dialog.ShowError(It.IsAny<string>()), Times.Once);
            Assert.Empty(_viewModel.Data);
        }

        [Fact]
        public void AddNew_Command_Should_Set_Empty_SelectedItem()
        {
            // Arrange
            _viewModel.SelectedItem = new Client { Id = 5, Name = "Old" };

            // Act
            _viewModel.AddNewCommand.Execute(null);

            // Assert
            Assert.NotNull(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem.Id);
            Assert.Equal(string.Empty, _viewModel.SelectedItem.Name);
        }

        [Fact]
        public void SaveCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var loadDataApiResult = new OperationResult<PagedResult<Client>>
            {
                Value = new PagedResult<Client>
                {
                    Results = new List<Client>
                    {
                        new Client { Id = 1, Name = "Test 1" },
                        new Client { Id = 2, Name = "Test 2" }
                    }
                }
            };
            var saveDataApiResult = new OperationResult();
            var clientToSave = new Client { Id = 1, Name = "Test" };

            _apiClientMock.Setup(client => client.Save(It.IsAny<Client>()))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 10))
                .ReturnsAsync(loadDataApiResult)
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(clientToSave);

            // Assert
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public void SaveCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var saveDataApiResult = new OperationResult
            {
                Errors = new List<string> { "Error" }
            };
            var clientToSave = new Client { Id = 1, Name = "Test" };

            _apiClientMock.Setup(client => client.Save(clientToSave))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(clientToSave);

            // Assert
            _apiClientMock.VerifyAll();
            _dialogProviderMock.Verify(dialog => dialog.ShowError(It.IsAny<string>()), Times.Once);
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void SaveCommand_can_execute_when_selected_item_is_not_null()
        {
            // Arrange
            _viewModel.SelectedItem = null;
            var canExecuteWithoutItem = _viewModel.SaveCommand.CanExecute(_viewModel.SelectedItem);

            // Act
            _viewModel.SelectedItem = new Client { Id = 1, Name = "Test" };
            var canExecuteWithItem = _viewModel.SaveCommand.CanExecute(_viewModel.SelectedItem);

            // Assert
            Assert.False(canExecuteWithoutItem);
            Assert.True(canExecuteWithItem);
        }

        [Fact]
        public void DeleteCommand_should_return_when_no_confirmation()
        {
            // Arrange
            var clientToDelete = new Client { Id = 1, Name = "Test" };

            _dialogProviderMock.Setup(dialog => dialog.Confirm(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(clientToDelete);

            // Assert
            _dialogProviderMock.VerifyAll();
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var loadDataApiResult = new OperationResult<PagedResult<Client>>
            {
                Value = new PagedResult<Client>
                {
                    Results = new List<Client>
                    {
                        new Client { Id = 2, Name = "Test 2" }
                    }
                }
            };
            var deleteApiResult = new OperationResult();
            var clientToDelete = new Client { Id = 1, Name = "Test" };

            _dialogProviderMock.Setup(dialog => dialog.Confirm(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            _apiClientMock.Setup(client => client.Delete(1))
                .ReturnsAsync(deleteApiResult)
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 10))
                .ReturnsAsync(loadDataApiResult)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(clientToDelete);

            // Assert
            _dialogProviderMock.VerifyAll();
            _apiClientMock.VerifyAll();
            Assert.Single(_viewModel.Data);
            Assert.Equal(2, _viewModel.Data[0].Id);
        }

        [Fact]
        public void DeleteCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var deleteApiResult = new OperationResult
            {
                Errors = new List<string> { "Error" }
            };
            var clientToDelete = new Client { Id = 1, Name = "Test" };

            _dialogProviderMock.Setup(dialog => dialog.Confirm(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            _apiClientMock.Setup(client => client.Delete(1))
                .ReturnsAsync(deleteApiResult)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(clientToDelete);

            // Assert
            _dialogProviderMock.VerifyAll();
            _apiClientMock.VerifyAll();
            _dialogProviderMock.Verify(dialog => dialog.ShowError(It.IsAny<string>()), Times.Once);
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteCommand_can_execute_when_selected_item_is_not_null_and_id_is_not_zero()
        {
            // Arrange
            _viewModel.SelectedItem = null;
            var canExecuteWithoutItem = _viewModel.DeleteCommand.CanExecute(_viewModel.SelectedItem);

            _viewModel.SelectedItem = new Client { Id = 0, Name = "New" };
            var canExecuteWithNewItem = _viewModel.DeleteCommand.CanExecute(_viewModel.SelectedItem);

            // Act
            _viewModel.SelectedItem = new Client { Id = 1, Name = "Existing" };
            var canExecuteWithSavedItem = _viewModel.DeleteCommand.CanExecute(_viewModel.SelectedItem);

            // Assert
            Assert.False(canExecuteWithoutItem);
            Assert.False(canExecuteWithNewItem);
            Assert.True(canExecuteWithSavedItem);
        }
    }
}
