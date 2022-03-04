var PairedUSBDevices = [];


export async function hookUpEvents() {
    navigator.hid.addEventListener('connect', ({ device }) => {
        console.log(`HID connected: ${device.productName}`);
    });

    navigator.hid.addEventListener('disconnect', ({ device }) => {
        console.log(`HID disconnected: ${device.productName}`);
    });
}

export async function requestDevice(filter)
{
    var objfilter = JSON.parse(filter);
    var devices = await navigator.hid.requestDevice(objfilter);
    console.log(devices);
    var device = devices[0];
    PairedUSBDevices.push(device);
    
    return returnHidDevice(device);
}

function returnHidDevice(device) {
    return {
        "ProductName": device.productName,
        "Id": device.vendorId + '-' + device.productId,
        "VendorId": device.vendorId,
        "ProductId": device.productId,
    };
}

export async function closeDevice(deviceId) {
    var device = devices.filter(function (item) {
        return item.id == deviceId;
    });
    await device.close();

    if (!device.opened) {
        await device.NotificationHandler.invokeMethodAsync('HandleOnDisconnected');
    }

}
export async function getDevices() {
    PairedUSBDevices = await navigator.hid.getDevices();
    console.log(PairedUSBDevices);
    return PairedUSBDevices.map(returnHidDevice);
}

export async function openDevice(deviceId,devicehandler) {
    var paireddevices = PairedUSBDevices.filter(function (device) {
        return device.vendorId + '-' + device.productId == deviceId;
    });
    
    var device = paireddevices[0]

    //Add NotificationHandler
    device.NotificationHandler = devicehandler;
    device.oninputreport = async (e) => {
        //console.log(e.data)
        await e.srcElement.NotificationHandler.invokeMethodAsync('HandleOnInputReport', e.reportId, new Uint8Array(e.data.buffer));
    };

    await device.open();

    if (device.opened) {
        await device.NotificationHandler.invokeMethodAsync('HandleOnConnected');
    }
        

    return device.opened;
}

export async function sendReport(deviceId,reportId, data) {
    var paireddevices = PairedUSBDevices.filter(function (device) {
        return device.vendorId + '-' + device.productId == deviceId;
    });

    var device = paireddevices[0]
    await device.sendReport(reportId,data);
}


export async function sendFeatureReport(deviceId, reportId, data) {
    var paireddevices = PairedUSBDevices.filter(function (device) {
        return device.vendorId + '-' + device.productId == deviceId;
    });    
    var device = paireddevices[0]
    try {
        await device.sendFeatureReport(reportId, data);
    }
    catch (err) {
        alert(err.name);
    }
   
 
}

