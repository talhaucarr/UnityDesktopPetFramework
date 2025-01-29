using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class ActiveWindowCollider : MonoBehaviour
{
    [SerializeField] private int windowRigidLayer = 0;
    [SerializeField] private int maxWindowColliderCount = 10;
    [SerializeField] private List<RECT> activeWindowRects = new();
    [SerializeField] private List<BoxCollider2D> boxColliders = new();
    
    private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr GetShellWindow();

    [DllImport("User32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);
    
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    private const int GWL_STYLE = -16;
    private const uint WS_MAXIMIZE = 0x01000000;
    private const uint WS_SIZEBOX = 0x00040000;
    
    private Camera _camera;
    
    private List<string> _windowTitles = new();

    private void Start()
    {
        _camera = Camera.main;
        
        InitializeBoxColliders();
        UpdateWindowsAndColliders();
    }

    private void Update()
    {
        UpdateWindowsAndColliders();
    }

    private void UpdateWindowsAndColliders()
    {
        activeWindowRects.Clear();
        _windowTitles.Clear();
        
        foreach (var window in GetOpenWindows())
        {
            if (!IsIconic(window.Key) && IsResizableWindow(window.Key) && !IsMaximizedWindow(window.Key))
            {
                if (GetWindowRect(new HandleRef(this, window.Key), out RECT rect) && (rect.Right - rect.Left) > 0 && (rect.Bottom - rect.Top) > 0)
                {
                    activeWindowRects.Add(rect);
                    _windowTitles.Add(window.Value);
                }
            }
        }
        
        UpdateBoxColliders();
    }

    private void InitializeBoxColliders()
    {
        for (int i = 0; i < maxWindowColliderCount; i++)
        {
            GameObject boxObject = new("WindowCollider")
            {
                layer = windowRigidLayer,
            };
            boxObject.transform.position = new Vector3(0, -100, 0);
            
            BoxCollider2D boxCollider = boxObject.AddComponent<BoxCollider2D>();
            Rigidbody2D rb = boxObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            
            boxColliders.Add(boxCollider);
        }
    }
    
    private void UpdateBoxColliders()
    {
        int count = Mathf.Min(activeWindowRects.Count, maxWindowColliderCount);
        for (int i = 0; i < maxWindowColliderCount; i++)
        {
            BoxCollider2D boxCollider = boxColliders[i];
            if (i < count)
            {
                RECT rect = activeWindowRects[i];
                Vector2 pixelSize = new(rect.Right - rect.Left, rect.Bottom - rect.Top);
                Vector2 screenPosition = new(rect.Left + pixelSize.x / 2, rect.Top + pixelSize.y / 2);

                Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, Screen.height - screenPosition.y, 0));
                worldPosition.z = 0;

                Vector3 worldSize = _camera.ScreenToWorldPoint(new Vector3(pixelSize.x, pixelSize.y, 0)) - _camera.ScreenToWorldPoint(Vector3.zero);
                
                boxCollider.gameObject.name = _windowTitles[i];
                
                boxCollider.size = new Vector2(worldSize.x, worldSize.y);
                boxCollider.attachedRigidbody.MovePosition(worldPosition);
            }
            else
            {
                boxCollider.size = Vector2.zero;
                boxCollider.attachedRigidbody.MovePosition(new Vector3(0, -15, 0));
                boxCollider.gameObject.name = "Inactive Window";
            }
        }
    }

    private static bool IsResizableWindow(IntPtr hWnd)
    {
        return (GetWindowLong(hWnd, GWL_STYLE) & WS_SIZEBOX) == WS_SIZEBOX;
    }
    
    private static bool IsMaximizedWindow(IntPtr hWnd)
    {
        return (GetWindowLong(hWnd, GWL_STYLE) & WS_MAXIMIZE) == WS_MAXIMIZE;
    }
    
    private static IDictionary<IntPtr, string> GetOpenWindows()
    {
        IntPtr shellWindow = GetShellWindow();
        Dictionary<IntPtr, string> windows = new();
        
        EnumWindows((hWnd, lParam) =>
        {
            if (hWnd == shellWindow || !IsWindowVisible(hWnd)) return true;
            
            int length = GetWindowTextLength(hWnd);
            if (length == 0) return true;

            StringBuilder builder = new(length + 1);
            GetWindowText(hWnd, builder, length + 1);
            string windowTitle = builder.ToString();

            if (!string.IsNullOrWhiteSpace(windowTitle))
            {
                windows[hWnd] = windowTitle;
            }
            return true;
        }, 0);

        return windows;
    }
}