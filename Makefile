.PHONY: build lint run publish msi all

SLN := WuTrayToggle.slnx

build:
	dotnet build $(SLN)

lint:
	dotnet format $(SLN) --verify-no-changes
	CI=true dotnet build $(SLN) --no-incremental

run:
	dotnet run --project src/WuTrayToggle

publish:
	dotnet publish src/WuTrayToggle -c Release -o publish

msi: publish
	dotnet build installer/WuTrayToggle.Installer.wixproj -c Release -p:PublishDir=$(CURDIR)/publish

all: lint
