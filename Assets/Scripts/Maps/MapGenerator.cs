using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;
using static Commons.Commons;
using System;
using Random = UnityEngine.Random;
using Controllers;
using Items;

namespace Maps
{

    /// <summary>
    ///  Used to generate map from pathBlocks
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        public float pathBlockWidth = 6; // (block scale x - character scale x)
        public int lineSeparation = 4; // how much line in a pathBlock

        public float coinRate = 0.8f; // rate for good item is a coin
        public float costumeRate = 0.05f; // rate for good item is a f@cking useless costume
        public float randomItemRate = 0.15f; // rate a random item appear
        public float speedItemRate = 0.2f; // rate a random item is a speed item (cannot appear in a good line)
        public float offsetItemHeight = 3f; // offset item height from line


        public float lineWidth;


        /// <summary>
        /// Defined pathBlocks template
        /// </summary>
        public PathBlock[] pathBlocksDefined;
        public ObjectPooling[] itemsDefined;
        public int avarageStraightLength;

        public PathBlock startBlock, hunterStartBlock;
        List<PathBlock> pathBlocks = new List<PathBlock>();



        private void Awake()
        {
            foreach (var item in itemsDefined)
            {
                item.SelfEnqueue();
            }
        }

        private void Start()
        {
            hunterStartBlock.SetNextPath(startBlock);
            //GeneratePathBlock();
        }

        //[Obsolete]
        //public PathBlock GetRandomPathBlock()
        //{
        //    int random = Random.Range(0, sumProbability);
        //    int sum = 0;
        //    for (int i = 0; i < pathBlocksDefined.Length; i++)
        //    {
        //        sum += pathBlocksDefined[i].Item2;
        //        if (random < sum)
        //            return pathBlocksDefined[i].Item1;
        //    }
        //    return null;
        //}

        public void ClearMap()
        {
            // Destroy all children in tranform, ignore startBlock and hunterStartBlock
            for (int i = 2; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        int straightBlockCount = 0;
        int lastCurvesIndex;

        /// <summary>
        /// Same as GetRandomPathBlock but each avarageStraightLength, it return at least avarageStraightLength-1 pathBlocksDefined[0] before another.
        /// Any other pathBlock never choosen twice
        /// </summary>
        /// <returns></returns>
        PathBlock GetPathBlockByRule()
        {
            if (straightBlockCount < avarageStraightLength + Random.Range(0, 3))
            {
                straightBlockCount++;
                return pathBlocksDefined[0];
            }
            int randIndex = Random.Range(1, pathBlocksDefined.Length);
            // choose another pathBlock if it is the same as last one
            while (randIndex == -lastCurvesIndex)
            {
                randIndex = Random.Range(1, pathBlocksDefined.Length);
            }
            if (randIndex == lastCurvesIndex) lastCurvesIndex = -randIndex;
            else lastCurvesIndex = randIndex;
            straightBlockCount = 0;
            return pathBlocksDefined[randIndex];
        }


        public void GeneratePathBlock(int mapLength = 20)
        {
            lineWidth = pathBlockWidth / (lineSeparation + 1);
            currentGoodLine = (lineSeparation - 1) / 2f;
            currentGoodLine -= Random.Range(0, lineSeparation);
            straightBlockCount = 0;
            PathBlock currentBlock = startBlock;
            for (int i = 0; i < mapLength; i++)
            {
                PathBlock nextBlock = GetPathBlockByRule().CloneNextPath(currentBlock, transform);
                currentBlock = nextBlock;
                GennerateItem(nextBlock);
                if(straightBlockCount == avarageStraightLength)
                {
                    // Don't write code like this. It's f@cking dumb. Just for deadline
                    nextBlock.GetComponent<TurretSpawner>()?.SpawnTurret();
                }
                //pathBlocks.Add(nextBlock);
            }
        }

        float currentGoodLine = 0;
        /// <summary>
        /// Generate coin or good item in a line. Good for gameplay.
        /// </summary>
        private void GennerateItem(PathBlock block)
        {
            float itemSpacing = block.PathLength / block.itemInLine;
            for (int i = 0; i < block.itemInLine; i++)
            {
                ChangeGoodLine();
                float itemTypeRate = Random.Range(0f, 1f);
                if (itemTypeRate < coinRate)
                {
                    var item = CoinItem.Spawn(transform);
                    item.transform.position = GetItemPosition(block, currentGoodLine, i, itemSpacing);
                }
                else if (itemTypeRate < coinRate + costumeRate)
                {
                    var item = UselessCostumeItem.Spawn(transform);
                    item.transform.position = GetItemPosition(block, currentGoodLine, i, itemSpacing);
                }
                // generate random item
                float randomRate = Random.Range(0f, 1f);
                if (randomRate < randomItemRate)
                {
                    float randomItemLine = CommonMath.SeparateRandom(-lineSeparation / 2f, lineSeparation / 2f, lineSeparation);
                    if (randomItemLine == currentGoodLine) return;
                    int randomItemIndex = Random.Range(0, itemsDefined.Length);
                    var item = itemsDefined[2].BlindSpawn(transform);
                    item.transform.position = GetItemPosition(block, randomItemLine, i, itemSpacing);

                }
            }

        }

        private Vector3 GetItemPosition(PathBlock block, float lineOffset, int itemIndex, float itemSpacing)
        {
            float distance = itemSpacing * itemIndex;
            Quaternion pathDirection = Quaternion.LookRotation(block.GetDirectionAtDistance(distance));
            Vector3 itemPosition = block.GetPointAtDistance(distance);
            var rotatedOffset = Quaternion.Euler(0, pathDirection.eulerAngles.y, 0) * (lineWidth * lineOffset * Vector3.right);
            //Debug.LogWarning(rotatedOffset);
            itemPosition += rotatedOffset;
            itemPosition += new Vector3(0, offsetItemHeight, 0);
            return itemPosition;
        }

        private void ChangeGoodLine()
        {
            // make sure player always can move in goodLine
            int goodLineMove = Random.Range(-1, 2);
            currentGoodLine += goodLineMove;
            var maxLine = lineSeparation - 1;
            currentGoodLine = Mathf.Clamp(currentGoodLine, maxLine / -2f, maxLine / 2f);
        }
    }

}
