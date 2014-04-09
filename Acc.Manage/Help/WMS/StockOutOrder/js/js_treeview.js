//<!--
var imgDir = "";
var indexOfEntries = new Array;
var nEntries = 0;
var auxs = new Array;
var folderOpenIcon = "folderOpen.gif";
var folderCloseIcon = "folderClose.gif";
var currentFolder = -1;
var targ = "";
function setFolderIcon(iOpen, iClose)
{
	if(!iOpen)
	{
		iOpen = "folderOpen.gif";
		iClose = "folderClose.gif";
	}

	if(!iClose)
		iClose = iOpen;

	folderOpenIcon = iOpen;
	folderCloseIcon = iClose;
}


function Folder(folderDescription, url)
{
	this.description = folderDescription;
	this.id = -1;
	this.navObj = 0;
	this.iconImg = 0;
	this.nodeImg = 0;
	this.isLastNode = 0;
	this.url = url;
	this.targ = targ;

	this.isOpen = false;
	this.iconSrc = folderCloseIcon;
	this.iconOpen = folderOpenIcon;
	this.iconClose = folderCloseIcon;
	this.children = new Array;
	this.nChildren = 0;

	this.initialize = initialize;
	this.setState = setState;
	this.addChild = addChild;
	this.createIndex = createEntryIndex;
	this.hide = hide;
	this.display = display;
	this.draw = draw;
//	this.subEntries = folderSubEntries;
}

function setState(isOpen)
{
	if (isOpen == this.isOpen)
		return;
	this.isOpen = isOpen;
	repaint(this);
}



function closeFolderIcon(folder)
{
	folder.iconImg.src = imgDir+folder.iconClose;
	folder.isOpen = false;
}


function repaint(folder)
{
	var i=0;
	if (folder.isOpen)
	{
		if (folder.nodeImg)
			if (folder.isLastNode){
				if(folder.nChildren>0)
					folder.nodeImg.src = imgDir + "Lminus.gif";
			}
			else{
				if( folder.nChildren>0)
					folder.nodeImg.src = imgDir + "Tminus.gif";
			}
		folder.iconImg.src = imgDir + folder.iconOpen;
		for (i=0; i<folder.nChildren; i++){
			folder.children[i].display();
		}
	}
	else
	{
		if (folder.nodeImg)
			if (folder.isLastNode){
				if( folder.nChildren>0)
					folder.nodeImg.src = imgDir + "Lplus.gif";
			}
			else{
				if( folder.nChildren>0)
					folder.nodeImg.src = imgDir + "Tplus.gif";
			}
		folder.iconImg.src = imgDir + folder.iconClose;
		for (i=0; i<folder.nChildren; i++){
			folder.children[i].hide();
			repaint(folder.children[i]);
		}
	}
}


function createEntryIndex()
{
	this.id = nEntries;
	indexOfEntries[nEntries] = this;
	nEntries++;
}


function initialize(level, lastNode, leftSide)
{
	var j=0;
	var i=0;
	var numberOfFolders;
	var numberOfDocs;
	var nc;

	nc = this.nChildren;
	this.createIndex();

	var auxEv = "";

	auxEv = "<a class='a-favMenu' href="+this.url;
	if( targ.length>0 && this.url != "#"){
			auxEv += " target='"+targ+"' ";
	}
	
	//if (this.description!='资源库管理'){		
		auxEv += " onclick='return clickOnNode("+this.id+");'";
	//}
	
	auxEv = auxEv + ' title=\"' + this.description +  '\">';
	if (level>0){
		if (lastNode){
			if( nc>0)
				nodeImage = "Lplus.gif";
			else
				nodeImage = "L.gif";
			this.draw(leftSide + auxEv + "<img name='nodeIcon" + this.id + "' src='" + imgDir + nodeImage+"' align=left border=0 vspace=0 hspace=0 border=0>");
			leftSide = leftSide + "<img src='" + imgDir + "blank.gif' align=left border=0 vspace=0 hspace=0>";
			this.isLastNode = 1;
		}
		else{
			if( nc>0)
				nodeImage = "Tplus.gif";
			else
				nodeImage = "T.gif";
			this.draw(leftSide + auxEv + "<img name='nodeIcon" + this.id + "' src='" + imgDir +nodeImage +"' align=left border=0 vspace=0 hspace=0 border=0>");
			leftSide = leftSide + "<img src='" + imgDir + "I.gif' align=left border=0 vspace=0 hspace=0>";
			this.isLastNode = 0;
		}
	}
	else
		this.draw("<a>"+auxEv);

	if (nc > 0)
	{
		level = level + 1;
		for (i=0 ; i < this.nChildren; i++)
		{
			if (i == this.nChildren-1)
				this.children[i].initialize(level, 1, leftSide);
			else
				this.children[i].initialize(level, 0, leftSide);
			}
	}
}


function draw(leftSide)
{
	document.write("<table  class='font-default' style='margin-left:2px' ");
	if(this.id==0)
		document.write("id='folder"+ this.id + "' style='display:block;' ");
	else
		document.write(" id='folder"+ this.id + "' style='display:none;' ");
	document.write(" border=0 cellspacing=0 cellpadding=0>");
	document.write("<tr valign=top><td nowrap>");
	document.write(leftSide);
	
	var auxEv="</a><a class='a-favMenu' href="+this.url+" onclick='return clickOnNode("+this.id+");'"+">";
	//alert(auxEv);
	document.write(auxEv+"<img name='folderIcon" + this.id + "' ");
	document.write("src='" + imgDir + this.iconSrc+"' align=left border=0 vspace=0 hspace=0 border=0></a>");
	document.write(auxEv+this.description + "</a>");//modify
	document.write("</td>");
	document.write("</table>");

	this.navObj = eval("folder"+this.id);
	this.iconImg = eval("folderIcon"+this.id);
	if( this.id>0)
		this.nodeImg =  eval("nodeIcon"+this.id);

}

function hide()
{
	if (this.navObj.style.display == "none")
		return;
	this.navObj.style.display = "none";
	this.setState(0);
	var i;
	for(i=0;i<this.nChildren;i++){
		this.children[i].hide();
	}
}

function display()
{
	this.navObj.style.display = "block";
}



function addChild(object)
{
	this.children[this.nChildren] = object;
	this.nChildren++;
	return object;
}


function clickOnNode(folderId)
{
	var clickedFolder = 0;
	var state = 0;
	var prevFolder ;
	if( currentFolder!=-1 && (currentFolder != folderId)){
		prevFolder = indexOfEntries[currentFolder];
		closeFolderIcon(prevFolder);
	}
	
	clickedFolder = indexOfEntries[folderId];
	state = clickedFolder.isOpen;
	if( clickedFolder.children.length>0 ){
		currentFolder = clickedFolder.id;
		clickedFolder.setState(!state);
	}
	else if(clickedFolder.children.length==0 && currentFolder!=folderId){
		currentFolder = clickedFolder.id;
		clickedFolder.setState(!state);
	}
	//if(!folderId)clickedFolder.setState(state);
	return true;
}

function treeAddLeaf(level, description, url)
{
	if(level)
		auxs[level] = auxs[level - 1].addChild(new Folder(description, url));
	else
		auxs[0] = new Folder(description,url);
}



function initializeDocument(){
	var foldersTree;
	foldersTree = auxs[0];
	foldersTree.initialize(0, 1, "");
}
//	loadImages();
//	foldersTree = auxs[0];
//	foldersTree.initialize(0, 1, "");
	//alert(auxs[0]);

//-->