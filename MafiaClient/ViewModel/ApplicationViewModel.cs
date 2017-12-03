using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MafiaClient.Model;

using CommonUtils;
namespace MafiaClient.ViewModel
{
  public class ApplicationViewModel: BaseViewModel
  {
    private BaseViewModel _currentVM;
    private BaseViewModel _roleVM;
    private BaseViewModel _clientVM;
    private MafiaClientModel _clientModel;
    public BaseViewModel CurrentViewModel
    {
      get { return _currentVM; }
      set { SetField(ref _currentVM, value, "CurrentViewModel"); }
    }

    public ApplicationViewModel()
    {
      _clientModel = new MafiaClientModel();
      CurrentViewModel = _clientVM = new ClientLoginViewModel(_clientModel);
      _clientModel.RoleSetEvent += _clientModel_RoleSetEvent;

    }

    private void _clientModel_RoleSetEvent(object sender, EventArgs args)
    {
      string role = sender as string;
      
      if (String.Compare(role, "mafia", true) == 0)
      {
        CurrentViewModel = _roleVM = new VillagerViewModel();

      }
    }
  }
}
  