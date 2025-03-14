﻿using Microsoft.Xna.Framework.Graphics;
using PicoMapper.Models;
using PicoMapper.States;
using System.Text.Json;

namespace PicoMapper;

public static class Utilities
{
    public static void Open(Mapper window)
    {
        try
        {
            using OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Pico Mapper Files (.p8m)|*.p8m";
            file.Title = "Open map.";

            DialogResult result = file.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (file.FileName.Trim() != string.Empty)
                {
                    string json = File.ReadAllText(file.FileName.Trim());
                    Map? map = JsonSerializer.Deserialize<Map>(json) ?? throw new JsonException("Invalid map data given.");

                    window.Context.SwitchState(new Editor(window, map, file.FileName.Trim()));
                }
            }
        }
        catch (Exception e)
        { 
            MessageBox.Show(
                $"Could not open file: {e.Message}",
                "Error opening file!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return;
        }
    }

    public static bool RefreshCache(Mapper window, Editor editor)
    { 
        editor.TileCache.Clear();
        foreach (Tile tile in editor.Map.Tiles)
        {
            Texture2D texture;
            try
            {
                using FileStream stream = new FileStream(tile.Path, FileMode.Open, FileAccess.Read);
                texture = Texture2D.FromStream(window.GraphicsDevice, stream);
            }
            catch (Exception err)
            {
                MessageBox.Show(
                    $"An error occoured parsing the texture: {err.Message}",
                    "Invalid Input!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );

                return false;
            }

            bool success = editor.TileCache.TryAdd(tile.ID, texture);
            if (!success)
            {
                MessageBox.Show(
                    "Something went wrong while trying to refresh tile cache. The app will now close.",
                    "Critical Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );

                window.Exit();
                return false;
            }
        }

        return true;
    }
}
