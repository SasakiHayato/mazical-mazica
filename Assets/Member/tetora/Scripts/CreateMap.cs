using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapState
{
    Wall, Floar, Player, Teleport
}
[System.Serializable]
public class CreateMap : MonoBehaviour
{
    [SerializeField]
    FieldScriptableObject _scriptableObject;
    [SerializeField]
    GameObject _wallObj;
    [SerializeField]
    GameObject _parentObj;
    [SerializeField]
    int _mapScale = 14;
    [SerializeField]
    int _releDis = 3;//何マス離すか

    [SerializeField]
    GameObject[] TESTTELEPORT;

    float _wallObjSize = 3;//マップ一つ一つのサイズ
    Map[,] _map;
    int _startDigPos;//掘り始める始点
    /// <summary>初期設定</summary>
    public void InitialSet()
    {
        //壁オブジェクトのScaleSizeを入れる
        _wallObjSize = _wallObj.transform.localScale.x;
        SetWall();
        StartDig();
        CreateEnemyPos();
        SetTeleportPos(TESTTELEPORT);
    }
    /// <summary>全てのマスを壁にする</summary>
    void SetWall()
    {
        _map = new Map[_scriptableObject.MapHorSide, _scriptableObject.MapVerSide];
        for (int y = 0; y < _scriptableObject.MapVerSide; y++)
        {
            for (int x = 0; x < _scriptableObject.MapHorSide; x++)
            {
                _map[x, y] = new Map(x + y * _scriptableObject.MapHorSide);
                _map[x, y].State = MapState.Wall;
            }
        }
    }
    /// <summary>ランダムな開始地点を決める</summary>
    int RandomPos()
    {
        int rndX = new System.Random().Next(1, (_scriptableObject.MapHorSide - 1) / 2) * 2 + 1;
        int rndY = new System.Random().Next(1, (_scriptableObject.MapVerSide - 1) / 2) * 2 + 1;
        return _startDigPos = rndX + rndY * _scriptableObject.MapHorSide;
    }
    /// <summary>4方向をランダムな順番で返す</summary>
    /// <param name="id">開始地点のid</param>
    /// <returns>4方向のid</returns>
    IEnumerable<(int twoTarget, int oneTarget)> CheckDir(int id)
    {
        (int twoTarget, int oneTarget)[] twoTargetDirs = {
            (id - (_scriptableObject.MapHorSide * 2),id - _scriptableObject.MapHorSide), //上
            (id + _scriptableObject.MapHorSide * 2,id + _scriptableObject.MapHorSide), //下
            (id + 2,id + 1), //右
            (id - 2,id - 1) //左
        };
        for (int i = 0; i < twoTargetDirs.Length; i++)
        {
            //順番をシャッフルする
            int r = UnityEngine.Random.Range(0, twoTargetDirs.Length);
            var tmp = twoTargetDirs[i];
            twoTargetDirs[i] = twoTargetDirs[r];
            twoTargetDirs[r] = tmp;
        }
        foreach ((int two, int one) dir in twoTargetDirs)
        {
            //two:2つ先のid ,one:1つ先のid
            if (dir.two < 0 || dir.two > _map.Length - 1)//範囲内のidのみ値を返す
            {
                continue;
            }
            //横のサイズを超えないか
            if (id % _scriptableObject.MapHorSide < 1 || id % _scriptableObject.MapHorSide >= _scriptableObject.MapHorSide - 1)
            {
                continue;
            }
            //2個先が壁か確認
            if (_map[dir.two % _scriptableObject.MapHorSide, dir.two / _scriptableObject.MapHorSide].State != MapState.Wall)
            {
                continue;
            }
            //1個先が壁か確認
            if (_map[dir.one % _scriptableObject.MapHorSide, dir.one / _scriptableObject.MapHorSide].State != MapState.Wall)
            {
                continue;
            }
            if (dir.two / _scriptableObject.MapHorSide == 0 || dir.two / _scriptableObject.MapHorSide == _scriptableObject.MapVerSide - 1)
            {
                continue;
            }
            if (dir.two % _scriptableObject.MapHorSide == 0 || dir.two % _scriptableObject.MapHorSide == _scriptableObject.MapHorSide - 1)
            {
                continue;
            }
            yield return dir;
        }
    }
    /// <summary>穴を掘る処理</summary>
    /// <param name="id">開始地点</param>
    void Dig(int id)
    {
        _map[id % _scriptableObject.MapHorSide, id / _scriptableObject.MapHorSide].State = MapState.Floar;
        foreach (var posId in CheckDir(id))
        {
            _map[posId.oneTarget % _scriptableObject.MapHorSide, posId.oneTarget / _scriptableObject.MapHorSide].State = MapState.Floar;
            Dig(posId.twoTarget);
        }
    }
    /// <summary>掘り始める</summary>
    void StartDig()
    {
        _startDigPos = RandomPos();
        Dig(_startDigPos);
        foreach (var pos in _map)
        {
            if (pos.State == MapState.Floar)
            {
                var emptyObj = new GameObject();//空のオブジェクトを生成
                emptyObj.name = "Floar";
                SetTransform(emptyObj, pos);
                continue;
            }
            else
            {
                var wall = Instantiate(_wallObj);
                SetTransform(wall, pos);
            }
        }
    }
    /// <summary>オブジェクトを並べる</summary>
    /// <param name="obj">並べたいオブジェクト</param>
    /// <param name="map">並べたいMap</param>
    void SetTransform(GameObject obj, Map map)
    {
        obj.transform.SetParent(_parentObj.transform);
        obj.transform.position = new Vector2(map.Id % _scriptableObject.MapHorSide - _scriptableObject.MapHorSide / 2,
            map.Id / _scriptableObject.MapHorSide - _scriptableObject.MapVerSide / 2) * _wallObjSize;
        map.ObjTransform = obj.transform;
    }
    void InstantiateEnemy()
    {
        for (int i = 0; i < _scriptableObject.EnemyObject.Count; i++)
        {
            GameObject enemy = _scriptableObject.EnemyObject[i];
        }
    }
    /// <summary>敵の生成する場所を決める</summary>
    void CreateEnemyPos()
    {
        int random = new System.Random().Next(0, GetFloar().Count);//床のランダムな場所を決める
        Map rndMap = GetFloar()[random];//生成する場所
        foreach (var item in _map)
        {
            if (rndMap.Id % _scriptableObject.MapHorSide - item.Id % _scriptableObject.MapHorSide < _releDis)//左側にプレイヤーがいるかどうか
            {
                CreateEnemy(rndMap);
            }
            if (item.Id % _scriptableObject.MapHorSide - rndMap.Id % _scriptableObject.MapHorSide < _releDis)//右側にプレイヤーがいるかどうか
            {
                CreateEnemy(rndMap);
            }
            if (rndMap.Id / _scriptableObject.MapHorSide - item.Id / _scriptableObject.MapHorSide < _releDis)//上側にプレイヤーがいるかどうか
            {
                CreateEnemy(rndMap);
            }
            if (item.Id / _scriptableObject.MapHorSide - rndMap.Id / _scriptableObject.MapHorSide < _releDis)//下側にプレイヤーがいるかどうか
            {
                CreateEnemy(rndMap);
            }
            else
            {
                CreateEnemyPos();
            }
        }
    }
    /// <summary>敵の生成</summary>
    /// <param name="map">生成するマス</param>
    void CreateEnemy(Map map)
    {
    }
    /// <summary>床オブジェクトを検索する</summary>
    /// <returns>床オブジェクトのList</returns>
    List<Map> GetFloar()
    {
        List<Map> generatablePosList = new List<Map>();//床の数
        foreach (var floar in _map)
        {
            if (floar.State == MapState.Floar)//Floarの場所を保存
            {
                generatablePosList.Add(floar);
            }
        }
        return generatablePosList;
    }
    /// <summary>Player生成の場所を決める</summary>
    public Transform DecisionPlayerPos()
    {
        int rndId = new System.Random().Next(0, GetFloar().Count);//床オブジェクトの入っているListの要素数からランダムな値を取得
        Map rndMap = GetFloar()[rndId];//床オブジェクトのランダムなオブジェクトを取得
        foreach (var item in _map)
        {
            if (item.Id + _scriptableObject.MapHorSide == rndMap.Id)
            {
                if (item.State == MapState.Wall)
                {
                    rndMap.State = MapState.Player;
                    return rndMap.ObjTransform;
                }
            }
        }
        Debug.Log($"rndId:{rndId},一個下のId{_map[(rndId + _scriptableObject.MapHorSide) % _scriptableObject.MapHorSide, (rndId + _scriptableObject.MapHorSide) / _scriptableObject.MapHorSide].Id}");
        return DecisionPlayerPos();
    }
    void SetTeleportPos(GameObject[] teleporterObj)
    {
        if (TESTTELEPORT != null)
        {
            return;
        }
        List<Map> leftMapsList = new List<Map>();
        List<Map> rightMapsList = new List<Map>();
        int leftLine = _scriptableObject.MapHorSide / 3; //3等分した時の左の線
        int rightLine = _scriptableObject.MapHorSide / 3 * 2;//3等分下時の右の線
        foreach (var item in GetFloar())
        {
            if (item.Id % _scriptableObject.MapHorSide <= leftLine)
            {
                leftMapsList.Add(item);
            }
            if (item.Id % _scriptableObject.MapHorSide >= rightLine)
            {
                rightMapsList.Add(item);
            }
        }
        int leftRnd = new System.Random().Next(0, leftMapsList.Count);
        int rightRnd = new System.Random().Next(0, rightMapsList.Count);
        var leftTele = Instantiate(TESTTELEPORT[0]);
        var rightTele = Instantiate(TESTTELEPORT[1]);
        leftTele.transform.position = leftMapsList[leftRnd].ObjTransform.position;
        rightTele.transform.position = rightMapsList[rightRnd].ObjTransform.position;
    }
    class Map
    {
        MapState _state = MapState.Wall;
        public MapState State { get => _state; set => _state = value; }
        Transform _objTransform;
        public Transform ObjTransform { get => _objTransform; set => _objTransform = value; }
        public readonly int Id;
        public Map(int id)
        {
            Id = id;
        }
    }
}
