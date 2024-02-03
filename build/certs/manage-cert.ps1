param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('add','remove')]
    [string]$action
)

$certPath = "./certs/server.crt"
$storeScope = "LocalMachine"
$storeName = "Root"

$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
$cert.Import($certPath)

$store = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeName, $storeScope)

$store.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)

if ($action -eq 'add') {
    $store.Add($cert)
    Write-Host "Certificate added to the $storeName store."
} elseif ($action -eq 'remove') {
    $store.Remove($cert)
    Write-Host "Certificate removed from the $storeName store."
}

$store.Close()