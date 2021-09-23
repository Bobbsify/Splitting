using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    public class AnimationHelper : MonoBehaviour
    {
        public void ChangeLayers(string changeAnimationString)
        //public void ChangeLayers(string[] names, int[] layerValues)
        {

        AnimationLayer[] layers = ParseStringToAnimationLayers(changeAnimationString);

            foreach (AnimationLayer layer in layers)
            {
                try
                {
                    transform.Find(layer.layerName).GetComponent<SpriteRenderer>().sortingOrder = layer.orderingLayerValue;
                }
                catch
                {
                    throw new Exception("Error: Could not find layer with name "+layer.layerName+" - Reminder: This check is case sensitive meaning you should use UpperCase letters correctly");
                }
            }
        }

    public void GiveCoordinates(string coordinates)
    {
        foreach (ObjectTransform objToEdit in ParseCoordinates(coordinates))
        {
            transform.Find(objToEdit.objectPath).position = objToEdit.position;
        }
    }


    //inserire le coordinate come "bone*X-Y-Z|bone*X-Y-Z|"
    private ObjectTransform[] ParseCoordinates(string coordinates)
    {
        List<ObjectTransform> result = new List<ObjectTransform>();

        string[] singleCoordinates = coordinates.Split('|');
        foreach (string coordinate in singleCoordinates)
        {
            string objectPath = coordinate.Split('*')[0];
            string[] vectorCoords = coordinate.Split('*')[1].Split('-');
            try
            {
                Vector3 position = new Vector3(float.Parse(vectorCoords[0]), float.Parse(vectorCoords[1]), float.Parse(vectorCoords[2]));
                ObjectTransform foundObj = new ObjectTransform(objectPath, position);
                result.Add(foundObj);
            }
            catch (FormatException e)
            {
                Debug.LogError("wrong values inputted into vector at " + objectPath + "\nfound values: " + vectorCoords + "\n" + e);
            }
        }

        return result.ToArray();
    }

    //String structure: "LayerName-orderingLayerNumber|LayerName-orderingLayerNumber|"..etc
    private AnimationLayer[] ParseStringToAnimationLayers(string str)
    {
        List<AnimationLayer> result = new List<AnimationLayer>();

        string[] resultString = str.Split('|'); //Divide Layer info into array

        foreach(string layerInfoStr in resultString)
        {
            string[] currentLayerInfo = layerInfoStr.Split('-');
            AnimationLayer layer = new AnimationLayer();
            try
            {
                layer.layerName = currentLayerInfo[0];
                int orderingLayerResult = 0; Int32.TryParse(currentLayerInfo[1], out orderingLayerResult);
                layer.orderingLayerValue = orderingLayerResult;
            }
            catch
            {
                throw new Exception("Wrong type has been given to the function, is the string format correct?");
            }
            result.Add(layer);
        }

        return result.ToArray();
    }

}

public class AnimationLayer
{
    public string layerName;
    public int orderingLayerValue;
}

public class ObjectTransform
{
    public string objectPath;
    public Vector3 position;

    public ObjectTransform(string objectPath, Vector3 position)
    {
        this.objectPath = objectPath;
        this.position = position;
    }
}