using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinaru.Services;
using Kinaru.Shared.DTOs.Messaging;
using System.Collections.ObjectModel;

namespace Kinaru.ViewModels;

[QueryProperty(nameof(UserId), "userId")]
public partial class MessagingViewModel : ObservableObject
{
    private readonly IMessagingService _messagingService;

    [ObservableProperty]
    private Guid userId;

    [ObservableProperty]
    private string newMessage = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string otherUserName = "Agent";

    public ObservableCollection<MessageDto> Messages { get; } = new();
    public ObservableCollection<ConversationDto> Conversations { get; } = new();

    [ObservableProperty]
    private bool isConversationView = true;

    public MessagingViewModel(IMessagingService messagingService)
    {
        _messagingService = messagingService;
    }

    partial void OnUserIdChanged(Guid value)
    {
        if (value != Guid.Empty)
        {
            IsConversationView = false;
            _ = LoadMessagesAsync();
        }
        else
        {
            IsConversationView = true;
            _ = LoadConversationsAsync();
        }
    }

    [RelayCommand]
    private async Task LoadConversationsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var conversations = await _messagingService.GetConversationsAsync();

            Conversations.Clear();
            foreach (var conversation in conversations)
            {
                Conversations.Add(conversation);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading conversations: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task LoadMessagesAsync()
    {
        if (IsBusy || UserId == Guid.Empty) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var messages = await _messagingService.GetMessagesAsync(UserId);

            Messages.Clear();
            foreach (var message in messages)
            {
                Messages.Add(message);
            }

            await _messagingService.MarkAsReadAsync(UserId);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading messages: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(NewMessage) || UserId == Guid.Empty) return;

        try
        {
            var messageDto = new CreateMessageDto
            {
                ReceiverId = UserId,
                Content = NewMessage
            };

            var sentMessage = await _messagingService.SendMessageAsync(messageDto);
            Messages.Add(sentMessage);

            NewMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error sending message: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        if (IsConversationView)
        {
            await LoadConversationsAsync();
        }
        else
        {
            await LoadMessagesAsync();
        }
    }

    [RelayCommand]
    private async Task OpenConversationAsync(Guid otherUserId)
    {
        await Shell.Current.GoToAsync($"messaging?userId={otherUserId}");
    }
}
