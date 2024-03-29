﻿//------------------------------------------------------------------------------------------------------------------
// Tunnel FX
// Copyright (c) Kronnect
//------------------------------------------------------------------------------------------------------------------

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;


namespace TunnelEffect
{


    [ExecuteInEditMode]
    public partial class TunnelFX2 : MonoBehaviour
    {

#if UNITY_EDITOR
        [MenuItem("GameObject/3D Object/TunnelFX2", false)]
        static void CreateTunnelMenuOption(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = new GameObject("Tunnel", typeof(MeshFilter), typeof(MeshRenderer));
            go.name = "Tunnel";
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            if (Selection.activeTransform != null)
            {
                go.transform.SetParent(Selection.activeTransform, false);
                go.transform.localPosition = Vector3.zero;
            }
            go.transform.localRotation = Quaternion.Euler(0, 0, 0);
            go.transform.localScale = new Vector3(10f, 10f, 950f);
            Selection.activeObject = go;
            go.AddComponent<TunnelFX2>();
        }


#endif

        [HideInInspector]
        public bool isDirty;


        // internal fields
        Material tunnelMat, tunnelMatTrans, tunnelMatOpaque, tunnelCapMat, tunnelCapTrans, tunnelCapOpaque;
        static string[] textureNames = new string[] { "_TunnelTex1", "_TunnelTex2", "_TunnelTex3", "_TunnelTex4" };
        static string[] paramNames = new string[] { "_Params1", "_Params2", "_Params3", "_Params4" };
        Dictionary<string, Texture2D> dict;
        string SKW_TUNNEL_BLEND_IN_ORDER = "TUNNEL_BLEND_IN_ORDER";
        string SKW_TUNNEL_LAYER_COUNT_1 = "TUNNEL_LAYER_COUNT_1";
        string SKW_TUNNEL_LAYER_COUNT_2 = "TUNNEL_LAYER_COUNT_2";
        string SKW_TUNNEL_LAYER_COUNT_3 = "TUNNEL_LAYER_COUNT_3";
        string SKW_TUNNEL_LAYER_COUNT_4 = "TUNNEL_LAYER_COUNT_4";
        List<string> shaderKeywords;
        float animationTime;
        Vector4[] tunnelParams = new Vector4[4];

        #region Game loop events

        void OnEnable()
        {
            if (tunnelMatTrans == null)
            {
                tunnelMatTrans = new Material(Shader.Find("TunnelEffect/TunnelFX2Alpha"));
                tunnelMatTrans.hideFlags = HideFlags.DontSave;
            }
            if (tunnelMatOpaque == null)
            {
                tunnelMatOpaque = new Material(Shader.Find("TunnelEffect/TunnelFX2"));
                tunnelMatOpaque.hideFlags = HideFlags.DontSave;
            }
            if (tunnelCapTrans == null)
            {
                tunnelCapTrans = new Material(Shader.Find("TunnelEffect/TunnelCapAlpha"));
                tunnelCapTrans.hideFlags = HideFlags.DontSave;
            }
            if (tunnelCapOpaque == null)
            {
                tunnelCapOpaque = new Material(Shader.Find("TunnelEffect/TunnelCap"));
                tunnelCapOpaque.hideFlags = HideFlags.DontSave;
            }

            if (dict == null)
            {
                dict = new Dictionary<string, Texture2D>();
            }

            UpdateMaterialProperties();
        }

        void OnDestroy()
        {
            if (tunnelMatOpaque != null)
            {
                DestroyImmediate(tunnelMatOpaque);
                tunnelMatOpaque = null;
            }
            if (tunnelMatTrans != null)
            {
                DestroyImmediate(tunnelMatTrans);
                tunnelMatTrans = null;
            }
            if (dict != null)
            {
                dict.Clear();
                dict = null;
            }
        }

        void Reset()
        {
            UpdateMaterialProperties();
        }

        void Update()
        {
            for (int k = 0; k < 4; k++)
            {
                if (Application.isPlaying)
                {
                    tunnelParams[k].x += _travelSpeed[k] * _layersSpeed * Time.deltaTime * 0.05f;
                    tunnelParams[k].y += _rotationSpeed[k] * _layersSpeed * Time.deltaTime * 0.05f;
                }
                tunnelMat.SetVector(paramNames[k], tunnelParams[k]);
            }
            if (Application.isPlaying && _positionAnimated)
            {
                animationTime += _layersSpeed * Time.deltaTime;
                float ox = Mathf.Sin(animationTime * 0.5f) * _animationAmplitude;
                float oy = Mathf.Cos(animationTime * 0.25f) * _animationAmplitude;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                Vector3 tpos = transform.TransformPoint(new Vector3(ox, oy, 10f / transform.localScale.z));
                transform.LookAt(tpos);
            }
        }


        #endregion

        void UpdateMaterialProperties()
        {
            if (_enableTransparency)
            {
                tunnelMat = tunnelMatTrans;
                tunnelCapMat = tunnelCapTrans;
            }
            else
            {
                tunnelMat = tunnelMatOpaque;
                tunnelCapMat = tunnelCapOpaque;
            }
            if (tunnelMat == null || tunnelCapMat == null)
                return;

            switch (_preset)
            {
                case TUNNEL_PRESET.SpaceTravel:
                    _layerCount = 4;
                    for (int k = 0; k < 4; k++)
                    {
                        _layerTextures[k] = GetTexture("starfield");
                    }
                    _alpha = new float[] { 0.533328f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 2f, 1.2f, 0.7f, 0.25f };
                    _rotationSpeed = new float[] { 0.7f, 0.35f, 0.2f, 0.1f };
                    _twist = new float[] { 0.1f, 0.1f, 0.1f, 0.1f };
                    _exposure = new float[] { 0.85f, 0.85f, 0.85f, 0.85f };
                    _fallOff = 1f;
                    _backgroundColor = new Color(100f / 255f, 253f / 255f, 206f / 255f);
                    _positionAnimated = true;
                    _animationAmplitude = 0.15f;
                    _blendInOrder = false;
                    _sides = 16;
                    _hyperSpeed = 0.634f;
                    _layersSpeed = 1f;
                    _textureScale = 1;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.MagmaTunnel:
                    _layerCount = 4;
                    _layerTextures[0] = GetTexture("chromatic");
                    for (int k = 1; k < 4; k++)
                    {
                        _layerTextures[k] = GetTexture("fire");
                    }
                    _alpha = new float[] { 0.533328f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 0.7f, 0.4f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { 0f, -0.002f, 0.003f, -0.005f };
                    _twist = new float[] { 0.2f, -0.2f, 0.2f, -0.2f };
                    _exposure = new float[] { 0.55f, 0.55f, 0.6f, 0.65f };
                    _fallOff = 1f;
                    _backgroundColor = new Color(1f, 1f, 0.89f);
                    _positionAnimated = false;
                    _animationAmplitude = 0.05f;
                    _blendInOrder = false;
                    _sides = 16;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 2;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.FireTornado:
                    _layerCount = 4;
                    for (int k = 0; k < 4; k++)
                    {
                        _layerTextures[k] = GetTexture("fire");
                    }
                    _alpha = new float[] { 0.533328f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 0.7f, 0.4f, 0.2f, 0.15f };
                    _rotationSpeed = new float[] { 4f, -5f, 6f, -7f };
                    _twist = new float[] { 0.5f, 0.5f, 0.5f, 0.5f };
                    _exposure = new float[] { 0.5f, 0.6f, 0.7f, 0.8f };
                    _fallOff = 1f;
                    _backgroundColor = new Color(1f, 1f, 0.89f);
                    _positionAnimated = true;
                    _animationAmplitude = 0.075f;
                    _blendInOrder = false;
                    _sides = 32;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 1;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.CloudAscension:
                    _layerCount = 4;
                    for (int k = 0; k < 4; k++)
                    {
                        _layerTextures[k] = GetTexture("clouds");
                    }
                    _alpha = new float[] { 0.533328f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 0.7f, 0.3f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { 0.2f, 0.18f, 0.16f, 0.1f };
                    _twist = new float[] { 0.05f, 0.04f, 0.03f, 0.02f };
                    _exposure = new float[] { 0.45f, 0.5f, 0.55f, 0.6f };
                    _fallOff = 1f;
                    _backgroundColor = new Color(0, 1f, 1f);
                    _positionAnimated = true;
                    _animationAmplitude = 0.05f;
                    _blendInOrder = false;
                    _sides = 16;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 1;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.MetalStructure:
                    _layerCount = 1;
                    _alpha = new float[] { 0.75f, 0.26664f, 0.13332f, 0.06666f };
                    _layerTextures[0] = GetTexture("metalstructure");
                    _travelSpeed = new float[] { 5f, 0.3f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { 0.2f, 0.18f, 0.16f, 0.1f };
                    _twist = new float[] { 0f, 0.04f, 0.03f, 0.02f };
                    _exposure = new float[] { 0.77f, 0.75f, 0.8f, 0.85f };
                    _fallOff = 0.7f;
                    _backgroundColor = Color.black;
                    _positionAnimated = true;
                    _animationAmplitude = 0.1f;
                    _blendInOrder = false;
                    _sides = 4;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 6;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.WaterTunnel:
                    _layerCount = 4;
                    _layerTextures[0] = GetTexture("water");
                    _layerTextures[1] = GetTexture("water2");
                    _layerTextures[2] = GetTexture("water");
                    _layerTextures[3] = GetTexture("water2");
                    _alpha = new float[] { 0.533328f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 0.7f, 0.3f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { 0.2f, 0.18f, 0.16f, 0.1f };
                    _twist = new float[] { 0.05f, 0.04f, 0.03f, 0.02f };
                    _exposure = new float[] { 0.5f, 0.6f, 0.6f, 0.6f };
                    _fallOff = 0.8f;
                    _backgroundColor = new Color(0, 1f, 1f);
                    _positionAnimated = true;
                    _animationAmplitude = 0.05f;
                    _blendInOrder = false;
                    _sides = 16;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 1;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.CavePassage:
                    _layerCount = 1;
                    _layerTextures[0] = GetTexture("rocks");
                    _alpha = new float[] { 1f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 2f, 0.3f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { 0f, 0.18f, 0.16f, 0.1f };
                    _twist = new float[] { 0f, 0.04f, 0.03f, 0.02f };
                    _exposure = new float[] { 0.7f, 0.75f, 0.8f, 0.85f };
                    _fallOff = 0.05f;
                    _backgroundColor = new Color(34 / 255f, 22 / 255f, 22 / 255f);
                    _positionAnimated = false;
                    _animationAmplitude = 0.1f;
                    _blendInOrder = false;
                    _sides = 4;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 4;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.Stripes:
                    _layerCount = 1;
                    _layerTextures[0] = GetTexture("stripes");
                    _alpha = new float[] { 1f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 2f, 0.3f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { 1f, 0.18f, 0.16f, 0.1f };
                    _twist = new float[] { 0f, 0.04f, 0.03f, 0.02f };
                    _exposure = new float[] { 0.57f, 0.75f, 0.8f, 0.85f };
                    _fallOff = 0.3f;
                    _backgroundColor = Color.black;
                    _positionAnimated = false;
                    _animationAmplitude = 0.1f;
                    _blendInOrder = false;
                    _sides = 32;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 3;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.Twightlight:
                    _layerCount = 3;
                    _layerTextures[0] = GetTexture("starfield");
                    _layerTextures[1] = GetTexture("lights2");
                    _layerTextures[2] = GetTexture("lights1");
                    _alpha = new float[] { 0.533328f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 2f, 0.3f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { 1f, 0.18f, 0.16f, 0.1f };
                    _twist = new float[] { 1f, 0.13f, -2.24f, 0.02f };
                    _exposure = new float[] { 0.7f, 0.832f, 0.7f, 0.85f };
                    _fallOff = 0.5f;
                    _backgroundColor = Color.white;
                    _positionAnimated = false;
                    _animationAmplitude = 0.1f;
                    _blendInOrder = false;
                    _sides = 32;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 2;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.MysticTravel:
                    _layerCount = 4;
                    _layerTextures[0] = GetTexture("starfield");
                    for (int k = 1; k < 4; k++)
                    {
                        _layerTextures[k] = GetTexture("clouds");
                    }
                    _alpha = new float[] { 0.533328f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 0.7f, 0.3f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { 0f, 0.18f, 0.16f, 0.1f };
                    _twist = new float[] { 0f, 0.04f, 0.03f, 0.02f };
                    _exposure = new float[] { 0.7f, 0.65f, 0.65f, 0.65f };
                    _fallOff = 1f;
                    _backgroundColor = new Color(155f / 255f, 225f / 255f, 1f);
                    _positionAnimated = false;
                    _animationAmplitude = 0.05f;
                    _blendInOrder = false;
                    _sides = 16;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 1;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.Chromatic:
                    _layerCount = 1;
                    _layerTextures[0] = GetTexture("chromatic");
                    _alpha = new float[] { 1f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 0.7f, 0.3f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { -0.15f, 0.18f, 0.16f, 0.1f };
                    _twist = new float[] { -0.4f, 0.04f, 0.03f, 0.02f };
                    _exposure = new float[] { 0.5f, 0.5f, 0.55f, 0.6f };
                    _fallOff = 1f;
                    _backgroundColor = Color.white;
                    _positionAnimated = false;
                    _animationAmplitude = 0.05f;
                    _blendInOrder = false;
                    _sides = 32;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 4;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.IcePassage:
                    _layerCount = 1;
                    _layerTextures[0] = GetTexture("ice");
                    _alpha = new float[] { 0.75f, 0.26664f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 0.7f, 0.3f, 0.2f, 0.1f };
                    _rotationSpeed = new float[] { 0f, 0.18f, 0.16f, 0.1f };
                    _twist = new float[] { 0f, 0f, 0f, 0f };
                    _exposure = new float[] { 0.67f, 0.5f, 0.55f, 0.6f };
                    _fallOff = 1f;
                    _backgroundColor = Color.white;
                    _positionAnimated = false;
                    _animationAmplitude = 0.05f;
                    _blendInOrder = false;
                    _sides = 4;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 4;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.ExhaustPort:
                    _layerCount = 2;
                    _layerTextures[0] = GetTexture("rustedmetal");
                    _layerTextures[1] = GetTexture("clouds");
                    _alpha = new float[] { 0.5f, 0.1f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 0.5f, 1f, 0.35f, 0.2f };
                    _rotationSpeed = new float[] { 0f, 0.5f, -0.5f, 0.5f };
                    _twist = new float[] { 0f, -0.2f, 0.2f, -0.2f };
                    _exposure = new float[] { 0.7f, 0.6f, 0.6f, 0.65f };
                    _fallOff = 0.4f;
                    _backgroundColor = Color.black;
                    _positionAnimated = false;
                    _animationAmplitude = 0.075f;
                    _blendInOrder = false;
                    _sides = 6;
                    _hyperSpeed = 0f;
                    _layersSpeed = 1f;
                    _textureScale = 3;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
                case TUNNEL_PRESET.CutOutRail:
                    _layerCount = 2;
                    _layerTextures[0] = GetTexture("coloredsand");
                    _layerTextures[1] = GetTexture("stripescutout");
                    _alpha = new float[] { 1f, 1f, 0.13332f, 0.06666f };
                    _travelSpeed = new float[] { 1f, 9f, 0.35f, 0.2f };
                    _rotationSpeed = new float[] { 0.1f, 0.1f, -0.5f, 0.5f };
                    _twist = new float[] { 0f, -1f, 0.2f, -0.2f };
                    _exposure = new float[] { 0.5f, 0.6f, 0.6f, 0.65f };
                    _fallOff = 10f;
                    _backgroundColor = Color.black;
                    _positionAnimated = false;
                    _animationAmplitude = 0.075f;
                    _blendInOrder = true;
                    _sides = 32;
                    _hyperSpeed = 0f;
                    _layersSpeed = 0.25f;
                    _textureScale = 1;
                    _capped = true;
                    _fallOffStart = 0;
                    _tintColor = Color.white;
                    break;
            }
            for (int k = 0; k < _layerTextures.Length; k++)
            {
                if (_layerTextures[k] == null)
                {
                    // try to get the texture from the material itself
                    _layerTextures[k] = (Texture2D)tunnelMat.GetTexture(textureNames[k]);
                    // or restore default texture (should not happen)
                    if (_layerTextures[k] == null)
                        _layerTextures[k] = GetTexture("starfield");
                }
                tunnelMat.SetTexture(textureNames[k], _layerTextures[k]);

                float expos = 0f;
                if (_exposure[k] < 0.5)
                {
                    expos = _exposure[k] * 2f;
                }
                else
                {
                    expos = 1f / (1.0001f - (_exposure[k] - 0.5f) * 2f);
                }
                tunnelParams[k].w = expos;
                tunnelParams[k].z = _twist[k] * 0.01f;
            }

            Color backgroundColor = _backgroundColor;
            backgroundColor.a *= _globalAlpha;
            _tintColor.a = _globalAlpha;
            tunnelMat.SetColor("_Color", _tintColor);
            tunnelMat.SetColor("_BackgroundColor", backgroundColor);
            tunnelCapMat.SetColor("_BackgroundColor", backgroundColor);
            Vector4 contributions = new Vector4(_alpha[0], _alpha[1], _alpha[2], _alpha[3]);
            tunnelMat.SetVector("_Params5", contributions);

            float fallOffStart = _fallOffStart * transform.localScale.z;
            tunnelMat.SetFloat("_FallOffStart", fallOffStart);

            float hsValue = Mathf.Clamp(1f - _hyperSpeed * Mathf.Clamp01(_layersSpeed), 0.001f, 1f);
            float fallOffEnd = _fallOff * transform.localScale.z;
            if (fallOffEnd < fallOffStart)
                fallOffEnd = fallOffStart;
            Vector4 mixParams = new Vector4(fallOffEnd,
                                                hsValue * transform.localScale.z / 100f,
                                                _textureScale,
                                                0);
            tunnelMat.SetVector("_MixParams", mixParams);


            if (shaderKeywords == null)
            {
                shaderKeywords = new List<string>();
            }
            else
            {
                shaderKeywords.Clear();
            }

            if (_blendInOrder && _layerCount > 1)
            {
                shaderKeywords.Add(SKW_TUNNEL_BLEND_IN_ORDER);
            }
            if (_layerCount == 4)
            {
                shaderKeywords.Add(SKW_TUNNEL_LAYER_COUNT_4);
            }
            else if (_layerCount == 3)
            {
                shaderKeywords.Add(SKW_TUNNEL_LAYER_COUNT_3);
            }
            else if (_layerCount == 2)
            {
                shaderKeywords.Add(SKW_TUNNEL_LAYER_COUNT_2);
            }
            else
            {
                shaderKeywords.Add(SKW_TUNNEL_LAYER_COUNT_1);
            }
            tunnelMat.shaderKeywords = shaderKeywords.ToArray();

            tunnelMat.renderQueue = _renderQueue;
            UpdateTunnelGeo();

            isDirty = true;
        }


        Texture2D GetTexture(string name)
        {
            if (dict.ContainsKey(name))
            {
                return dict[name];
            }

            Texture2D tex = Resources.Load<Texture2D>("Textures/" + name);
            dict[name] = tex;
            return tex;
        }


        void UpdateTunnelGeo()
        {

            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mf == null || mr == null)
            {
                Debug.LogError("Create TunnelFX using top menu GameObject > 3D Object > TunnelFX");
                DestroyImmediate(this);
                return;
            }
            mr.enabled = _globalAlpha > 0;

            Mesh mesh = new Mesh();
            mesh.hideFlags = HideFlags.DontSave;
            mesh.subMeshCount = 3;

            int vertexCount = sides * 2;
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();

            // Add sides
            const int numSegments = 1;
            for (int i = 0; i < numSegments; i++)
            {
                for (int v = 0, s = 0; s <= _sides; s++, v += 2)
                {
                    float a = s * Mathf.PI * 2f / _sides + Mathf.PI * 0.25f;
                    float x = Mathf.Cos(a);
                    float y = Mathf.Sin(a);
                    vertices.Add(new Vector3(x, y, -1f + (2f * i) / numSegments));
                    vertices.Add(new Vector3(x, y, -1f + (2f * (i + 1f) / numSegments)));
                    uv.Add(new Vector2((float)i / numSegments, (1f / vertexCount) * v));
                    uv.Add(new Vector2(((float)i + 1) / numSegments, (1f / vertexCount) * v));
                }
            }
            int triangleCount = (_sides + 1) * 6;
            int[] indices = new int[triangleCount * numSegments];
            for (int i = 0, s = 0, k = 0; k < numSegments; k++)
            {
                for (int t = 0; t < _sides; t++, i += 6, s += 2)
                {
                    indices[i] = s;
                    indices[i + 1] = s + 1;
                    indices[i + 2] = s + 2;
                    indices[i + 3] = s + 2;
                    indices[i + 4] = s + 1;
                    indices[i + 5] = s + 3;
                }
                s += 2;
            }

            // Add cap front and back
            int capIndex = vertices.Count;
            vertices.Add(new Vector3(0, 0, -1f));
            vertices.Add(new Vector3(0, 0, 1f));
            uv.Add(Vector2.zero);
            uv.Add(Vector2.zero);
            mesh.vertices = vertices.ToArray();
            mesh.SetTriangles(indices, 0);
            if (_capped)
            {
                List<int> indicesCap = new List<int>();
                for (int s = 0; s < _sides * 2; s += 2)
                {
                    indicesCap.Add(s);
                    indicesCap.Add(s + 2);
                    indicesCap.Add(capIndex);
                }
                mesh.SetTriangles(indicesCap.ToArray(), 1);
                indicesCap.Clear();
                int offset = (numSegments - 1) * (_sides + 1) * 2 + 1;
                for (int s = 0; s < _sides * 2; s += 2)
                {
                    indicesCap.Add(s + offset); // + 1);
                    indicesCap.Add(s + offset + 2); // 3);
                    indicesCap.Add(capIndex + 1);
                }
                mesh.SetTriangles(indicesCap.ToArray(), 2);
            }

            mesh.uv = uv.ToArray();
            mesh.RecalculateNormals();

            mf.sharedMesh = mesh;
            mr.sharedMaterials = new Material[] { tunnelMat, tunnelCapMat, tunnelCapMat };
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows = false;

            MeshCollider mc = gameObject.GetComponent<MeshCollider>();
            if (mc != null)
            {
                mc.sharedMesh = mesh;
            }

        }


    }

}