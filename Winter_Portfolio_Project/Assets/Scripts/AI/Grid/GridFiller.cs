using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WPP.DRAWING;

namespace WPP.GRID
{
    // 모노 비헤이비어 상속시켜서 Grid 변경시켜주는 방식으로 사용
    // 유닛이 심길 수 있는 위치를 반환해주는 클레스를 하나 더 만들어보기
    public class GridFiller : MonoBehaviour
    {
        // 블루팀 레드팀의 경우를 나눠서 적용해야함 --> 그러니깐 4개가 나오겠지

        // 처음에 채울 좌표를 들고 있는 배열
        // 해제될 좌표를 들고 있는 배열

        // 이렇게 구성해놓으면 관리하기 좋을 듯?

        // TopRight, BottomLeft 이런 식으로 두 좌표만 놓고 그 사이의 그리드를 채워주는 방식 ㄱㄱㄱ
        public struct AreaData
        {
            Vector2Int _topRight;
            public Vector2Int TopRight { get { return _topRight; } }

            Vector2Int _bottomLeft;
            public Vector2Int BottomLeft { get { return _bottomLeft; } }

            bool _nowFill;
            public bool NowFill { get { return _nowFill; } }

            public AreaData(Vector2Int topRight, Vector2Int bottomLeft, bool nowFill)
            {
                _topRight = topRight;
                _bottomLeft = bottomLeft;
                _nowFill = nowFill;
            }
        }

        DrawingData rNoDestroyDrawingData = new DrawingData(
            new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 17), new Vector3(14, 0, 17), new Vector3(14, 0, 0) },
            new int[] { 0, 1, 2, 0, 2, 3}
        );

        DrawingData rAllDestroyDrawingData = new DrawingData(
            new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 12), new Vector3(14, 0, 12), new Vector3(14, 0, 0) },
            new int[] { 0, 1, 2, 0, 2, 3 }
        );

        DrawingData rLeftDestroyDrawingData = new DrawingData(
            new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 12), new Vector3(7, 0, 12), new Vector3(7, 0, 17), new Vector3(14, 0, 17), new Vector3(14, 0, 12), new Vector3(14, 0, 0) },
            new int[] { 0, 1, 6, 1, 5, 6, 5, 2, 3, 5, 3, 4 }
        );

        DrawingData rRightDestroyDrawingData = new DrawingData(
            new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 12), new Vector3(0, 0, 17), new Vector3(7, 0, 17), new Vector3(7, 0, 12), new Vector3(14, 0, 12), new Vector3(14, 0, 0) },
            new int[] { 0, 1, 6, 1, 5, 6, 1, 3, 4, 1, 2, 3 }
        );



        DrawingData cNoDestroyDrawingData = new DrawingData(
            new Vector3[] { new Vector3(0, 0, 33), new Vector3(14, 0, 33), new Vector3(14, 0, 16), new Vector3(0, 0, 16) },
            new int[] { 0, 1, 3, 3, 1, 2 }
        );

        DrawingData cAllDestroyDrawingData = new DrawingData(
            new Vector3[] { new Vector3(0, 0, 33), new Vector3(14, 0, 33), new Vector3(14, 0, 21), new Vector3(0, 0, 21) },
            new int[] { 0, 1, 3, 3, 1, 2 }
        );

        DrawingData cLeftDestroyDrawingData = new DrawingData(
            new Vector3[] { new Vector3(0, 0, 33), new Vector3(14, 0, 33), new Vector3(14, 0, 21), new Vector3(14, 0, 16), new Vector3(7, 0, 16), new Vector3(7, 0, 21), new Vector3(0, 0, 21) },
            new int[] { 0, 1, 6, 6, 1, 2, 5, 2, 4, 4, 2, 3 }
        );

        DrawingData cRightDestroyDrawingData = new DrawingData(
            new Vector3[] { new Vector3(0, 0, 33), new Vector3(14, 0, 33), new Vector3(14, 0, 21), new Vector3(7, 0, 21), new Vector3(7, 0, 16), new Vector3(0, 0, 16), new Vector3(0, 0, 21) },
            new int[] { 0, 1, 6, 6, 1, 2, 5, 6, 3, 5, 3, 4 }
        );


        AreaData cAreaData = new AreaData(new Vector2Int(14, 33), new Vector2Int(0, 16), true);

        AreaData cLeftAreaData = new AreaData(new Vector2Int(7, 21), new Vector2Int(0, 17), false);
        AreaData cRightAreaData = new AreaData(new Vector2Int(14, 21), new Vector2Int(7, 17), false);

        AreaData rFillData = new AreaData(new Vector2Int(14, 17), new Vector2Int(0, 0), true);

        AreaData rLeftAreaData = new AreaData(new Vector2Int(7, 16), new Vector2Int(0, 12), false);
        AreaData rRightAreaData = new AreaData(new Vector2Int(14, 16), new Vector2Int(7, 12), false);

        public Func<Grid[,]> OnReturnGridRequested;
        [SerializeField] SpawnAreaDrawer _spawnRect;
        [SerializeField] TowerCondition _storedTowerCondition;

        private void Start()
        {
            GridStorage gridStorage = GetComponent<GridStorage>();
            if (gridStorage == null) return;

            OnReturnGridRequested = gridStorage.ReturnGridArray;
        }

        // 상대 진형에 오브젝트 못 심게 막아버리기
        public void OnLandFormationAssigned(LandFormation landFormation)
        {
            ResetGrid(OnReturnGridRequested(), landFormation);
        }

        // 타워가 파괴될 때, 상대 진형 심을 수 있는 범위를 바꾸기
        public void OnTowerConditionChanged(LandFormation landFormation, TowerCondition towerCondition)
        {
            ResetGrid(OnReturnGridRequested(), landFormation, towerCondition);
            // 여기서 _spawnRect 이걸 초기화 해주자
            // 이후에 스폰될 때 초기화된 데이터를 불러와서 사용하면 될 듯
        }

        public void OnBuildingPlanted(RectInt rectInt)
        {
            AreaData data = ReturnFillData(rectInt, true);
            ResetArea(OnReturnGridRequested(), data); // filldata는 다시 만들어줘야 할 듯?
        }

        public void OnBuildingReleased(RectInt rectInt)
        {
            AreaData data = ReturnFillData(rectInt, false);
            ResetArea(OnReturnGridRequested(), data);
        }

        AreaData ReturnFillData(RectInt rectInt, bool nowFill)
        {
            AreaData data = new AreaData(rectInt.max, rectInt.min, nowFill);
            return data;
        }

        void ResetArea(Grid[,] grids, AreaData data)
        {
            // 만약 그리드의 사이즈가 맞지 않는다면 더 이상 진행하면 안 될듯?
            for (int x = data.BottomLeft.x; x < data.TopRight.x; x++)
            {
                for (int y = data.BottomLeft.y; y < data.TopRight.y; y++)
                {
                    grids[x, y].IsFill = data.NowFill;
                }
            }
        }

        // 이렇게 주면 해당 구역 재설정하는 것임
        // 다음 코드에 추가로 스폰 가능 구역 표시도 다시 그려주기
        void ResetGrid(Grid[,] grids, LandFormation myLandFormation)
        {
            _storedTowerCondition = TowerCondition.NoDestroy;

            if (myLandFormation == LandFormation.C) // 내 진형 기준으로 반대편 진형을 초기화 해줘야함
            {
                ResetArea(grids, rFillData);
                _spawnRect.Initialize(rNoDestroyDrawingData);
            }
            else if (myLandFormation == LandFormation.R)
            {
                ResetArea(grids, cAreaData);
                _spawnRect.Initialize(cNoDestroyDrawingData);
            }
        }

        // 어느 진형을 바꿀지 + 어느 쪽을 바꿀지 지정
        void ResetGrid(Grid[,] grids, LandFormation myLandFormation, TowerCondition myTowerCondition)
        {
            // 만약 그리드의 사이즈가 맞지 않는다면 더 이상 진행하면 안 될듯?

            if(_storedTowerCondition == TowerCondition.NoDestroy)
            {
                _storedTowerCondition = myTowerCondition;

                if (myLandFormation == LandFormation.C)
                {
                    if (_storedTowerCondition == TowerCondition.LeftDestroy)
                    {
                        ResetArea(grids, rLeftAreaData);
                        _spawnRect.Initialize(rLeftDestroyDrawingData);
                    }
                    else if (_storedTowerCondition == TowerCondition.RightDestroy)
                    {
                        ResetArea(grids, rRightAreaData);
                        _spawnRect.Initialize(rRightDestroyDrawingData);
                    }
                }
                else if (myLandFormation == LandFormation.R)
                {
                    if (_storedTowerCondition == TowerCondition.LeftDestroy)
                    {
                        ResetArea(grids, cLeftAreaData);
                        _spawnRect.Initialize(cLeftDestroyDrawingData);
                    }
                    else if (_storedTowerCondition == TowerCondition.RightDestroy)
                    {
                        ResetArea(grids, cRightAreaData);
                        _spawnRect.Initialize(cRightDestroyDrawingData);
                    }
                }
            }
            else if(_storedTowerCondition == TowerCondition.LeftDestroy && myTowerCondition == TowerCondition.RightDestroy)
            {
                _storedTowerCondition = TowerCondition.AllDestroy;

                if (myLandFormation == LandFormation.C)
                {
                    ResetArea(grids, rRightAreaData);
                    _spawnRect.Initialize(rAllDestroyDrawingData);
                }
                else if (myLandFormation == LandFormation.R)
                {
                    ResetArea(grids, cRightAreaData);
                    _spawnRect.Initialize(cAllDestroyDrawingData);
                }
            }
            else if (_storedTowerCondition == TowerCondition.RightDestroy && myTowerCondition == TowerCondition.LeftDestroy)
            {
                _storedTowerCondition = TowerCondition.AllDestroy;

                if (myLandFormation == LandFormation.C)
                {
                    ResetArea(grids, rLeftAreaData);
                    _spawnRect.Initialize(rAllDestroyDrawingData);
                }
                else if (myLandFormation == LandFormation.R)
                {
                    ResetArea(grids, cLeftAreaData);
                    _spawnRect.Initialize(cAllDestroyDrawingData);
                }
            }
        }
    }
}
