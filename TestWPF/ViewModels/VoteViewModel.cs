using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommonUtils;
using System.Windows.Input;

namespace TestWPF.ViewModels
{
  public class VoteViewModel: BaseViewModel
  {
    private string _chatContent;
    private string _chatBlockContent;
    ObservableCollection<string> _characters = new ObservableCollection<string>() { "x", "y", "z"};
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

    public ObservableCollection<string> Characters
    {
      get { return _characters; }
    }

    public VoteViewModel()
    {
      ChatBlockContent = "";
      EnterKeyCommand = new DelegateCommand(OnEnter);
    }


    public void OnEnter(object par)
    {
      ChatBlockContent += "\nMyself: " + _chatContent;
      ChatContent = "";
    }
  }
}
