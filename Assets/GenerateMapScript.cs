using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GenerateMapScript : MonoBehaviour
{
    [SerializeField] private Tilemap _til;
    [SerializeField] private Tile _tile;

    private Vector2[] _directions = new Vector2[4] { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    private List<Location> _locations;


    private int _maxRooms = 0;
    private int _quantityRooms = 0;

    private void Start()
    {
        _maxRooms = 9;
        _locations = new List<Location>();

        GenerateNewRoom(new Vector2(0, 0), -1, null);
    }

    private void GenerateNewRoom(Vector2 centralPosition, int cutWallId, Location prevLocation) 
    {
        TypeRooms typeLocation = TypeRooms.Default;
        if (_quantityRooms == 0)
        {
            typeLocation = TypeRooms.Spawn;
        }
        else if (_quantityRooms > 0 && _quantityRooms < _maxRooms - 1)
        {
            int randomRoom = Random.Range(0, 100);
            if (randomRoom >= 0 && randomRoom <= 75)
            {
                typeLocation = TypeRooms.Default;
            }
            else if (randomRoom > 75 && randomRoom <= 90 && _locations.Count(item => item.TypeRoom == TypeRooms.Loot) < _maxRooms / 5)
            {
                typeLocation = TypeRooms.Loot;
            }
            else if (randomRoom > 90 && randomRoom <= 100 && _locations.Count(item => item.TypeRoom == TypeRooms.Store) < _maxRooms / 5)
            {
                typeLocation = TypeRooms.Store;
            }
        }
        else if (_quantityRooms == _maxRooms - 1)
        {
            typeLocation = TypeRooms.Boss;
        }

        var allLocations = Resources.LoadAll<GameObject>($"Locations/{typeLocation}"); 
        var locationObject = Instantiate(allLocations[Random.Range(0, allLocations.Length)], new Vector3(0, 0, 0), Quaternion.identity);

        var tilemap = locationObject.GetComponentsInChildren<Tilemap>()[1];
        tilemap.CompressBounds();

        Location location = null;
        switch (cutWallId)
        {
            case 0:
                locationObject.transform.position = new Vector2(centralPosition.x - tilemap.size.x, centralPosition.y + (tilemap.size.y - 1) / 2);

                location = new Location(locationObject, typeLocation, prevLocation.LocationPosition + Vector2.left);
                break;
            case 1:
                locationObject.transform.position = new Vector2(centralPosition.x - (tilemap.size.x - 1) / 2, centralPosition.y);

                location = new Location(locationObject, typeLocation, prevLocation.LocationPosition + Vector2.down);
                break;
            case 2:
                locationObject.transform.position = new Vector2(centralPosition.x, centralPosition.y + (tilemap.size.y - 1) / 2);

                location = new Location(locationObject, typeLocation, prevLocation.LocationPosition + Vector2.right);
                break;
            case 3:
                locationObject.transform.position = new Vector2(centralPosition.x - (tilemap.size.x - 1) / 2, centralPosition.y + tilemap.size.y);

                location = new Location(locationObject, typeLocation, prevLocation.LocationPosition + Vector2.up);
                break;
            default:
                location = new Location(locationObject, typeLocation, new Vector2(0, 0));
                break;
        }

        _locations.Add(location);

        foreach (var item in _locations)
        {
            print(item.LocationPosition);
        }
        print("-----");

        _quantityRooms++;

        if (cutWallId >= 0 && cutWallId < 4)
        {
            CutWall(tilemap, cutWallId);
            location.Bridges[cutWallId] = true; 
        }

        for (int i = 0; i < 4; i++)
        {
            var chanceToSpawnBridge = Random.Range(0, 9);

            if (location.Bridges[i] == false && chanceToSpawnBridge <= 3 && _quantityRooms < _maxRooms &&
                !_locations.Any(item => item.LocationObject != location.LocationObject && item.LocationPosition == location.LocationPosition + _directions[i]))
            {
                var locationPos = SetBridge(tilemap, i);

                GenerateNewRoom(locationPos, (i + 2) % 4, location);
            }
            else if (prevLocation == null && i == 3 && _quantityRooms < _maxRooms)
            {
                for (int a = 0; a < _locations.Count; a++)
                {
                    for (int j = 0; j < _locations[a].Bridges.Length; j++)
                    {
                        if (_locations[a].Bridges[j] == false && _quantityRooms < _maxRooms &&
                            !_locations.Any(item => item.LocationObject != _locations[a].LocationObject && item.LocationPosition == _locations[a].LocationPosition + _directions[j]))
                        {
                            var locationPos = SetBridge(_locations[a].LocationObject.GetComponentsInChildren<Tilemap>()[1], j);

                            GenerateNewRoom(locationPos, (j + 2) % 4, _locations[a]);
                        }
                    }
                }
            }
        }
    }

    private Vector2 SetBridge(Tilemap tilemap, int bridgeId)
    {
        CutWall(tilemap, bridgeId);

        Vector3 bridgePosition = new Vector3(0, 0, 0);
        switch (bridgeId)
        {
            case 0:
                bridgePosition = new Vector3(tilemap.transform.position.x + tilemap.size.x,
                                             tilemap.transform.position.y - ((tilemap.size.y - 1) / 2 - 2),
                                             0);
                break;
            case 1:
                bridgePosition = new Vector3(tilemap.transform.position.x + ((tilemap.size.x - 1) / 2 - 2),
                                             tilemap.transform.position.y,
                                             0);
                break;
            case 2:
                bridgePosition = new Vector3(tilemap.transform.position.x,
                                             tilemap.transform.position.y - ((tilemap.size.y - 1) / 2 + 3),
                                             0);
                break;
            case 3:
                bridgePosition = new Vector3(tilemap.transform.position.x + ((tilemap.size.x - 1) / 2 + 3),
                                             tilemap.transform.position.y - tilemap.size.y,
                                             0);
                break;
            default:
                break;
        }

        var bridge = Instantiate(Resources.Load<GameObject>("Locations/Bridge"), bridgePosition, Quaternion.identity);
        bridge.transform.rotation = Quaternion.Euler(0, 0, 90 * bridgeId);

        var bridgeSize = bridge.GetComponentsInChildren<Tilemap>()[1];
        bridgeSize.CompressBounds();

        Vector2 locationPos = new Vector2(0, 0);
        switch (bridgeId)
        {
            case 0:
                locationPos = new Vector2(bridge.transform.position.x + bridgeSize.size.x,
                                          bridge.transform.position.y - (bridgeSize.size.y - 1) / 2);
                break;
            case 1:
                locationPos = new Vector2(bridge.transform.position.x + (bridgeSize.size.y - 1) / 2,
                                          bridge.transform.position.y + bridgeSize.size.x - 1);
                break;
            case 2:
                locationPos = new Vector2(bridge.transform.position.x - bridgeSize.size.x,
                                          bridge.transform.position.y + (bridgeSize.size.y - 1) / 2 + 1);
                break;
            case 3:
                locationPos = new Vector2(bridge.transform.position.x - (bridgeSize.size.y - 1) / 2 - 1,
                                          bridge.transform.position.y - bridgeSize.size.x);
                break;
            default:
                break;
        }

        return locationPos;
    }

    private void CutWall(Tilemap tilemap, int wallId)
    {
        switch (wallId)
        {
            case 0:
                tilemap.SetTile(new Vector3Int(tilemap.size.x - 1, -((tilemap.size.y - 1) / 2 - 1) - 1, 0), null);
                tilemap.SetTile(new Vector3Int(tilemap.size.x - 1, -((tilemap.size.y - 1) / 2) - 1, 0), null);
                tilemap.SetTile(new Vector3Int(tilemap.size.x - 1, -((tilemap.size.y - 1) / 2 + 1) - 1, 0), null);
                break;
            case 1:
                tilemap.SetTile(new Vector3Int((tilemap.size.x - 1) / 2 - 1, -1, 0), null);
                tilemap.SetTile(new Vector3Int((tilemap.size.x - 1) / 2, -1, 0), null);
                tilemap.SetTile(new Vector3Int((tilemap.size.x - 1) / 2 + 1, -1, 0), null);
                break;
            case 2:
                tilemap.SetTile(new Vector3Int(0, -((tilemap.size.y - 1) / 2 - 1) - 1, 0), null);
                tilemap.SetTile(new Vector3Int(0, -((tilemap.size.y - 1) / 2) - 1, 0), null);
                tilemap.SetTile(new Vector3Int(0, -((tilemap.size.y - 1) / 2 + 1) - 1, 0), null);
                break;
            case 3:
                tilemap.SetTile(new Vector3Int((tilemap.size.x - 1) / 2 - 1, -tilemap.size.y, 0), null);
                tilemap.SetTile(new Vector3Int((tilemap.size.x - 1) / 2, -tilemap.size.y, 0), null);
                tilemap.SetTile(new Vector3Int((tilemap.size.x - 1) / 2 + 1, -tilemap.size.y, 0), null);
                break;
            default:
                break;
        }
    }
}

public class Location
{
    public Location(GameObject locationObject, TypeRooms typeRoom, Vector2 locationPosition)
    {
        LocationObject = locationObject;
        TypeRoom = typeRoom;
        LocationPosition = locationPosition;

        Bridges = new bool[4];
    }

    public GameObject LocationObject { get; private set; }
    public TypeRooms TypeRoom { get; private set; }
    public Vector2 LocationPosition { get; private set; }
    public bool[] Bridges { get; set; }
}