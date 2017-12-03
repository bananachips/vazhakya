using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MafiaClient.Model;
using CommonUtils;

namespace MafiaClient.ViewModel
{
  class ChatViewModel: BaseViewModel
  {
    private MafiaClientModel _mafiaClientModel;
    private string _chatContent;
    private string _chatBlockContent;
    public ICommand EnterKeyCommand{get; set;}
    
    public string ChatContent
    {
      get { return _chatContent; }
      set { SetField(ref _chatContent, value, "ChatContent"); }
    }

    public string ChatBlockContent
    {
      get { return _chatBlockContent; }
      set { SetField(ref _chatBlockContent, value, "ChatBlockContent"); }
    }



    public ChatViewModel(MafiaClientModel cm)
    {
      _mafiaClientModel = cm;
      ChatBlockContent = "";
      EnterKeyCommand = new DelegateCommand(OnEnter);
    }


    public void OnEnter(object par)
    {
      ChatBlockContent += "\nMyself: " + _chatContent;
      _mafiaClientModel.SendMessage("ChatMessage", _chatContent);
      ChatContent = "";
      
    }
  }
}
