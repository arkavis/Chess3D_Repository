using UnityEngine;
using System.Collections;

public class GlobalMessage : MonoBehaviour {
	//board properties
	private static int row;
	private static int column;
	public static void setRow(int r){row = r;}
	public static int getRow(){return row;}
	public static void setColumn(int c){column = c;}
	public static int getColumn(){return column;}
	
	private static Vector3 currentPosition;
	private static string currentPlayerName;
	public static void setCurrentPosition(Vector3 position){ currentPosition = position;}
	public static Vector3 getCurrentPosition(){ return currentPosition;}
	public static void setPlayerName(string name){ currentPlayerName = name;}
	public static string getPlayerName(){ return currentPlayerName;}
	
}
