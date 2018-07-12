using UnityEngine;
using System.Collections;
using ch.sycoforge.Decal;

namespace ch.sycoforge.Decal.Demo
{
    /// <summary>
    /// Demo class showing how to register a <c>StaticProxyCollection</c>.
    /// </summary>
    public class ProxyRegister : MonoBehaviour
    {

        //------------------------------------
        // Exposed Fields
        //------------------------------------

        //Assign the proxy collection for the level here
        public StaticProxyCollection ProxyCollection;


        //------------------------------------
        // Methods
        //------------------------------------

        private void Start()
        {
            // Register the proxy collection
            EasyDecal.SetStaticProxyCollection(ProxyCollection);
        }
    }
}
