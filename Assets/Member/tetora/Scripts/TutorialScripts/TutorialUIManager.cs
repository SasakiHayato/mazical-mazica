using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UIManagement;

public class TutorialUIManager : MonoBehaviour, IGameSetupable
{
    [SerializeField]
    TutorialFieldManager _fieldManager;
    [SerializeField]
    CharacterManager _characterManager;
    [SerializeField]
    TaskManager _taskManager;
    [SerializeField]
    RawMaterialData _materialData;
    [SerializeField]
    PlayerStatusPanel _playerStatusPanel;
    [SerializeField]
    List<Popup> _popupList;
    [SerializeField]
    Text _taskText;
    [SerializeField]
    Text _inputText;

    int IGameSetupable.Priority => 2;

    static List<Popup> s_popupList;
    public static TutorialUIManager Instance { get; private set; }

    void Awake()
    {
        s_popupList = _popupList;
        GameController.Instance.AddGameSetupable(this);
        Instance = this;
    }

    void IGameSetupable.GameSetup()
    {
        //��ʂɕ\������f�މ摜��ݒ�
        if (_fieldManager)
        {
            _fieldManager.MaterialList
            .Subscribe(items =>
            {
                List<RawMaterialDatabase> materialDatas = new List<RawMaterialDatabase>();
                items.ForEach(item => materialDatas.Add(_materialData.GetMaterialData(item)));
                _playerStatusPanel.SetMaterialSprite(materialDatas);
            })
            .AddTo(_fieldManager);
        }

        //Player���������ꂽ��Player�̏���Player�̃X�e�[�^�X��\������N���X�ɓn��
        if (_characterManager)
        {
            _characterManager.PlayerSpawn
            .Subscribe(p =>
            {
                _playerStatusPanel.SetSlider(p);
                _playerStatusPanel.SetMaterialViewPanel(p);
            })
            .AddTo(_characterManager);
        }
        ChangeTaskText(0);
    }

    /// <summary>
    /// ���ꂩ����^�X�N��ς���
    /// </summary>
    /// <param name="id"></param>
    public void ChangeTaskText(int id)
    {
        Debug.Log($"ID:{id}");
        _taskText.text = _taskManager.TaskList[id].TaskText;
        _inputText.text = _taskManager.TaskList[id].InputText;
    }

    public static Popup FindPopup(string path)
    {
        return s_popupList.Find(p => p.Path == path);
    }
}