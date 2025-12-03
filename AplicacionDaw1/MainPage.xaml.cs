namespace AplicacionDaw1
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiService _apiService;

        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            await LoadCharacters();
        }

        private async Task LoadCharacters()
        {
            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                CounterBtn.IsEnabled = false;

                var characters = await _apiService.GetCharactersAsync();

                if (characters != null && characters.Count > 0)
                {
                    // Filtrar solo los que tienen imagen
                    var validCharacters = characters
                        .Where(c => !string.IsNullOrEmpty(c.Image))
                        .ToList();

                    CharactersCollectionView.ItemsSource = validCharacters;
                    CharactersCollectionView.IsVisible = true;

                    await DisplayAlert("Éxito",
                        $"Se cargaron {validCharacters.Count} personajes",
                        "OK");
                }
                else
                {
                    await DisplayAlert("Info", "No se encontraron personajes", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                CounterBtn.IsEnabled = true;
            }
        }
    }
}