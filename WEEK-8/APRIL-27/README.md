````markdown
# Azure Virtual Network & Virtual Machine Setup

This repository documents the task of setting up an Azure Virtual Network, deploying Azure Virtual Machines inside the network, and exploring nested virtualization.

## Task Overview

The main objectives of this task are:

1. Create and configure an Azure Virtual Network  
2. Deploy Virtual Machines inside the Virtual Network  
3. Explore running Virtual Machines inside another VM  

## Topics Covered

- Azure Virtual Network setup  
- Subnet configuration  
- Azure Virtual Machine deployment  
- Network Security Group configuration  
- VM-to-VM communication  
- Nested virtualization  

## 1. Azure Virtual Network Setup

### Objective

Create a secure Azure Virtual Network to allow Azure resources to communicate with each other.

### Steps

1. Create a new Virtual Network in Azure.  
2. Define the address space.  

   Example:

   ```text
   10.0.0.0/16
````

3. Create one or more subnets.

   Example:

   ```text
   10.0.1.0/24
   ```

4. Configure Network Security Groups.

5. Attach the subnet to the Virtual Network.

6. Verify that the Virtual Network is created successfully.

## 2. Azure Virtual Machines on Azure Virtual Network

### Objective

Deploy Azure Virtual Machines and connect them to the created Virtual Network.

### Steps

1. Create a new Azure Virtual Machine.

2. Select the existing Virtual Network.

3. Choose the required subnet.

4. Configure public and private IP settings.

5. Add inbound security rules for:

   * SSH for Linux VMs
   * RDP for Windows VMs

6. Deploy the VM.

7. Verify VM connectivity.

## 3. VM-to-VM Communication

### Objective

Verify communication between Virtual Machines within the same Virtual Network.

### Steps

1. Deploy two VMs inside the same Virtual Network.
2. Connect to one VM using SSH or RDP.
3. Ping the private IP address of the second VM.
4. Test SSH or RDP access between the VMs if required.
5. Confirm that both VMs can communicate internally.

## 4. Running Virtual Machines on Other VMs

### Objective

Explore nested virtualization by running a Virtual Machine inside another Azure Virtual Machine.

### Requirements

Nested virtualization requires supported Azure VM sizes such as:

* Dv3 series
* Ev3 series
* Later compatible VM families

### Steps

1. Create a supported Azure VM.

2. Enable virtualization support.

3. Install a hypervisor on the host VM.

   Examples:

   * Hyper-V for Windows
   * KVM for Linux
   * VirtualBox where supported

4. Create a guest Virtual Machine inside the Azure VM.

5. Start the guest VM.

6. Test its performance and connectivity.

## Repository Structure

```text
azure-vnet-vm-setup/
│
├── README.md
├── screenshots/
│   ├── vnet-setup.png
│   ├── vm-configuration.png
│   └── nested-virtualization.png
│
└── notes/
    └── observations.md
```

## Expected Outcome

After completing this task, the following outcomes should be achieved:

* Understanding of Azure Virtual Network basics
* Ability to create and configure subnets
* Ability to deploy Azure Virtual Machines inside a Virtual Network
* Understanding of Network Security Groups
* Testing of internal communication between VMs
* Basic understanding of nested virtualization in Azure

## Notes

* Use private IP addresses for internal VM communication
* Allow only required ports in NSG rules
* Avoid exposing unnecessary services to the internet
* Nested virtualization may require higher VM sizes and more resources
* Stop unused VMs to avoid extra Azure charges

## Conclusion

This task helps in understanding how Azure networking and virtualization work together. It also provides practical exposure to deploying Virtual Machines inside a Virtual Network and experimenting with nested virtualization.

```
```
