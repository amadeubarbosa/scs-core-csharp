
MSBUILD=msbuild /nologo
BUILD=$(MSBUILD) /p:Configuration=Release /verbosity:minimal
CLEAN=$(MSBUILD) /p:Configuration=Release /verbosity:minimal /target:Clean
_TEST="%VS90COMNTOOLS%..\IDE\MSTest.exe"
TEST="$(_TEST)" /nologo /noresults

build: build-base build-examples
test: build run-tests
clean: clean-base clean-example
rebuild: clean build

build-base:
	$(BUILD) scs.sln

build-examples:
	cd demo\pingpong
	$(BUILD) PingPong.sln
	cd ..
	cd hello
	$(BUILD) HelloWorld.sln
	cd ..\..
	
run-tests:
	cd Test\bin\Release
	$(TEST) /testcontainer:Test.dll
	cd ..\..\..

clean-base:
	$(CLEAN) scs.sln

clean-example:
	cd demo\pingpong
	$(CLEAN) PingPong.sln
	cd ..
	cd hello
	$(CLEAN) HelloWorld.sln
	cd ..\..
	
dist: clean-example
	if exist package rm -r package	
	mkdir package	
	mkdir package\docs	
	cp doc\Scs-doc.XML package\docs		
	xcopy demo\* package\demo /e /i /q
	cp lib\*.dll lib\*.xsd lib\generated\*.dll package
	zip -r scs.zip package -q
	rm -r package
