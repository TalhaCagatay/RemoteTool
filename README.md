# Download and Parse Service

This is a simple download and parse service for Unity that provides a generic method to download JSON data from a given URL and parse it into the specified object type. The service is designed to handle common scenarios such as error handling, timeout settings, and storing the latest fetched data.

Development purely made for upm. All files are under Packages->RemoteTool

This package also includes a MainThreadDispatcher script. Sample project uses this to make sure remote config update events fires from the main thread.

## Features

- **Generic Download and Parse Method:** Download and parse JSON data into the specified object type.
- **Error Handling:** Proper error messages and handling for failed download and parsing operations.
- **Timeout Setting:** Configurable timeout for the HTTP request.
- **Latest Fetched Data Storage:** The service stores the latest fetched JSON data for immediate retrieval.

## Usage

1. **Download the Service:**
   - Clone the repository (This option allows to test the sample scene)
   
   OR
   - Use UPM to install the package => https://github.com/TalhaCagatay/RemoteTool.git?path=Packages/RemoteTool
     
  <a href="https://imgbb.com/"><img src="https://i.ibb.co/VqQcTFJ/Screenshot-2024-02-09-at-14-24-30.png" alt="Screenshot-2024-02-09-at-14-24-30" border="0"></a><br /><a target='_blank' href='https://tr.imgbb.com/'></a><br />

3. **Usage Example:**
   - Call the `DownloadAndParse` method, providing the URL and the expected object type.

   ```csharp
   using UnityEngine;
   using System.Threading.Tasks;
   using Madbox;

   public class Example : MonoBehaviour
   {
       public class MyDTO
       {
           public string key;
           public string value;
       }

       private MyDTO _defaultValue = new MyDTO{key="defaultKey", value = "defaultValue"};
   
       private async void Start()
       {
           MyDTO deserializedDTO = await RemoteTool.DownloadAndParse("https://example.com/api/data.json", _defaultValue);
           // Use the DTO values as needed
       }
   }
   ```

## Sample Project
   - Samples folder has a scene which you can play and check out the example. [Here](https://docs.google.com/spreadsheets/d/1JfMfWWK2jIH4lgM9lzM35CCeY9n9tLfdwmqidt5F48M/edit#gid=687366319) is the google sheet to modify the values and see the changes. Changes updates itself every 3 seconds so you don't have to restart the play mode.
     <a href="https://imgbb.com/"><img src="https://i.ibb.co/X41YwQj/Screenshot-2024-02-09-at-14-40-37.png" alt="Screenshot-2024-02-09-at-14-40-37" border="0"></a><br /><a target='_blank' href='https://tr.imgbb.com/'></a><br />
   - Project is a basic gameplay where player can run, climb and fly depending the surface types. Just like the game Pocket Champs. You can modify the each movement type's speed from the google sheet file to see the changes.
     <a href="https://imgbb.com/"><img src="https://i.ibb.co/B3Y7ns1/Screenshot-2024-02-09-at-15-05-15.png" alt="Screenshot-2024-02-09-at-15-05-15" border="0"></a><br /><a target='_blank' href='https://tr.imgbb.com/'></a><br />
   - Fog settings can also be set from the google sheet.
   <a href="https://ibb.co/ZJ9mmjN"><img src="https://i.ibb.co/xsZHHk3/Screenshot-2024-02-09-at-14-33-21.png" alt="Screenshot-2024-02-09-at-14-33-21" border="0"></a><br /><a target='_blank' href='https://tr.imgbb.com/'></a><br />
   - When you start the sample scene, it will first fetch the remote config files and then starts playing. Check out the init logs to see.
   <a href="https://imgbb.com/"><img src="https://i.ibb.co/mGgH7D3/Screenshot-2024-02-09-at-14-59-57.png" alt="Screenshot-2024-02-09-at-14-59-57" border="0"></a><br /><a target='_blank' href='https://tr.imgbb.com/'></a><br />

## Development Time
This package took about 5 hours to complete.
## Ideas to Improve
  - Download and parse method currently uses newtonsoft and it can be improved by accepting a custom parser.
  - Download and parse method uses HTTPClient meaning it does not depend on monobehaviours and unity web requests. This can be hugely improved with a plugin like UniTask.
  - Sample project has a basic dependency injection implementation for creating and handling both controllers and configs. For a larger project, a plugin like zenject can be used.
  - Sample project fetches remote config every 3 seconds with using c# Tasks and requires to be handling the edge cases for cancelling the request manually when play mode is exit. Again, UniTask can be used to improve.
