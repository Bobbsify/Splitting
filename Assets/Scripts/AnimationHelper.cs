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
            /*
            for (int i = 0; i < names.Length; i++)
            {
                try
                {
                    transform.Find(names[i]).GetComponent<SpriteRenderer>().sortingOrder = layerValues[i];
                }
                catch
                {
                    throw new Exception("Error: Could not find layer with name " + names[i] + " - Reminder: This check is case sensitive meaning you should use UpperCase letters correctly");
                }
            }
            */
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