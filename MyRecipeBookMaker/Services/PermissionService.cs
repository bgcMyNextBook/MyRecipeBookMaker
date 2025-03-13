using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Platform;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MyRecipeBookMaker.Services
{
    public interface IPermissionService
    {
        Task<bool> RequestPermissionAsync<TPermission>() where TPermission : BasePermission, new();
        Task<PermissionStatus> CheckAndRequestStoragePermission();
    }
    
    static public class PermissionService
    {
        static public async Task<bool> RequestPermissionAsync<TPermission>() where TPermission : BasePermission, new()
        {
            var permission = new TPermission();
            PermissionStatus s = await permission.RequestAsync();
            return true;
        }

        static public async Task<PermissionStatus> CheckAndRequestStorageWritePermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            return status;
        }
        static public async Task<PermissionStatus> CheckAndRequestStorageReadPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();

            return status;
        }

    }
}