using UnityEngine;
using System.Collections;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;
    public GameObject Target = null;

    private BasicMazeGenerator mMazeGenerator = null;

	void Start () {
		//if (!FullRandom) {
		//	Random.seed = RandomSeed;
		//}
		//switch (Algorithm) {
		//case MazeGenerationAlgorithm.PureRecursive:
		//	mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
		//	break;
		//case MazeGenerationAlgorithm.RecursiveTree:
		//	mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
		//	break;
		//case MazeGenerationAlgorithm.RandomTree:
		//	mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
		//	break;
		//case MazeGenerationAlgorithm.OldestTree:
		//	mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
		//	break;
		//case MazeGenerationAlgorithm.RecursiveDivision:
		//	mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
		//	break;
		//}
		//mMazeGenerator.GenerateMaze ();
		//for (int row = 0; row < Rows; row++) {
		//	for(int column = 0; column < Columns; column++){
		//		float x = column*(CellWidth+(AddGaps?.2f:0));
		//		float z = row*(CellHeight+(AddGaps?.2f:0));
		//		MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
		//		GameObject tmp;
  //              GameObject tmp2;
  //              tmp = Instantiate(Floor) as GameObject;
		//		tmp.transform.parent = transform;
  //              tmp.transform.localPosition = new Vector3(100, 0, z);

  //              if (cell.WallRight){
		//			tmp = Instantiate(Wall,  new Vector3(x+CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,90,0)) as GameObject;// right
		//			tmp.transform.parent = transform;
		//		}
		//		if(cell.WallFront){
		//			tmp = Instantiate(Wall, transform.position + new Vector3(x,0,z+CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,0,0)) as GameObject;// front
		//			tmp.transform.parent = transform;
		//		}
		//		if(cell.WallLeft){
		//			tmp = Instantiate(Wall, transform.position + new Vector3(x-CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,270,0)) as GameObject;// left
		//			tmp.transform.parent = transform;
		//		}
		//		if(cell.WallBack){
		//			tmp = Instantiate(Wall, transform.position + new Vector3(x,0,z-CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,180,0)) as GameObject;// back
		//			tmp.transform.parent = transform;
		//		}
		//		if(cell.IsGoal ){
  //                  //tmp2 = Instantiate(GoalPrefab,transform.position+ new Vector3(x,1,z), Quaternion.Euler(0,0,0)) as GameObject;
  //                  Target.transform.localPosition = transform.position + new Vector3(x, 0.177f, z);
		//		}



                

  //          }
		//}
		//if(Pillar != null){
		//	for (int row = 0; row < Rows+1; row++) {
		//		for (int column = 0; column < Columns+1; column++) {
		//			float x = column*(CellWidth+(AddGaps?.2f:0));
		//			float z = row*(CellHeight+(AddGaps?.2f:0));
		//			GameObject tmp = Instantiate(Pillar, transform.position + new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
		//			tmp.transform.parent = transform;
		//		}
		//	}
		//}
	}


    public void DestroyMaze()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }





    }



    public void CreateMaze()
    {
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }
        mMazeGenerator.GenerateMaze();
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;
                
                tmp = Instantiate(Floor) as GameObject;
                tmp.transform.parent = transform;
                tmp.transform.localPosition = new Vector3(x, 0, z);
                tmp.layer = 9;

                if (cell.WallRight)
                {
                    tmp = Instantiate(Wall) as GameObject;// right
                    tmp.transform.parent = transform;
                    tmp.transform.localPosition = new Vector3(x + CellWidth / 2, 0, z)+Wall.transform.position;
                    tmp.transform.localRotation= Quaternion.Euler(0, 90, 0);
                    tmp.layer = 9;
                }
                if (cell.WallFront)
                {
                    tmp = Instantiate(Wall) as GameObject;// front
                    tmp.transform.parent = transform;
                    tmp.transform.localPosition = new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position;
                    tmp.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    tmp.layer = 9;
                }
                if (cell.WallLeft)
                {
                    tmp = Instantiate(Wall) as GameObject;// left
                    tmp.transform.parent = transform;
                    tmp.transform.localPosition = new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position;
                    tmp.transform.localRotation = Quaternion.Euler(0, 270, 0);
                    tmp.layer = 9;
                }
                if (cell.WallBack)
                {
                    tmp = Instantiate(Wall) as GameObject;// back
                    tmp.transform.parent = transform;
                    tmp.transform.localPosition = new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position;
                    tmp.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    tmp.layer = 9;
                }
                if (cell.IsGoal)
                {
                    //tmp2 = Instantiate(GoalPrefab,new Vector3(x,1,z), Quaternion.Euler(0,0,0)) as GameObject;
                    Target.transform.localPosition = new Vector3(x, 0.177f, z);

                }





            }
        }
        if (Pillar != null)
        {
            for (int row = 0; row < Rows + 1; row++)
            {
                for (int column = 0; column < Columns + 1; column++)
                {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));
                    GameObject tmp = Instantiate(Pillar) as GameObject;
                    tmp.transform.parent = transform;
                    tmp.transform.localPosition =  new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2);
                    tmp.transform.localRotation = Quaternion.identity;
                    tmp.layer = 9;


                }
            }
        }





    }
}
