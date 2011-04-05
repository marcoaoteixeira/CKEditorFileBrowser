/* ****************************************************************************
// Executa ações quando o DOM estiver carregado por completo.
**************************************************************************** */
jQuery(document).ready(function () {

	setPanelsLayout();

	configFileBrowserCreateFolderDialog();
	configFileBrowserDeleteItemDialog();

	renderFileBrowserTreeView();

	configFileBrowserTreeViewExtraControls();

	configUploadify();
	bindUploadify();

	bindCKEditorSelectItem();
});
/* ****************************************************************************
// Renderiza a árvore de arquivos do navegador. Com base na pasta raiz que foi
// definida na configuração do sistema.
**************************************************************************** */
var setPanelsLayout = function () {

	var body = jQuery('body');

	body.layout({
		north: {
			resizable: false,
			slidable: false,
			closable: false,
			size: 100
		},
		west: {
			fxName: "slide",
			fxSettings: {
				duration: 500,
				easing: "easeOutBounce"
			},
			size: 500
		},
		south: {
			resizable: false,
			slidable: false,
			closable: false,
			size: 50
		}
	});
};
/* ****************************************************************************
// Renderiza a árvore de arquivos do navegador. Com base na pasta raiz que foi
// definida na configuração do sistema.
**************************************************************************** */
var renderFileBrowserTreeView = function () {

	var treeViewContainer = jQuery('div.treeview-container');

	jQuery.ajax({
		beforeSend: function (jqXHR, settings) {
			treeViewContainer.block({ message: null });
		},
		cache: false,
		dataType: 'html',
		error: function (jqXHR, textStatus, errorThrown) {
			alert(errorThrown.Message);
		},
		success: function (data, textStatus, jqXHR) {
			treeViewContainer.html(data);
			setFileBrowserTreeViewBehavior();
			setFileBrowserTreeViewEvents();
			treeViewContainer.unblock();
		},
		url: 'Handlers/RenderFileBrowserTreeView.ashx'
	});
};
/* ****************************************************************************
// Configura o comportamento de árvore do elemento DIV.treeview
**************************************************************************** */
var setFileBrowserTreeViewBehavior = function () {

	var treeView = jQuery('ul.treeview');

	treeView.treeview({
		collapsed: true,
		animated: 'fast',
		control: 'ul.treeview-controls'
	});
};
/* ****************************************************************************
// Configura os eventos dos elementos do tree view.
**************************************************************************** */
var setFileBrowserTreeViewEvents = function () {

	var treeView = jQuery('ul.treeview');

	treeView.find('span').click(function () {

		treeView.find('span').removeClass('selected');

		jQuery(this).addClass('selected');

		var item = getFileBrowserTreeViewSelectedItemPath();
		var folder = getFileBrowserTreeViewSelectedItemPath('folder');

		jQuery('li.selected-item').find('span').html(item);
		
		configUploadifyFolder(folder);

		return false;
	});
};
/* ****************************************************************************
// Configura os controles adicionais do tree view, como adicionar pasta,
// remover arquivo, etc.
**************************************************************************** */
var configFileBrowserTreeViewExtraControls = function () {

	var controls = jQuery('ul.treeview-controls');

	controls.find('a.refresh').click(function () {
		renderFileBrowserTreeView();

		return false;
	});

	controls.find('a.add').click(function () {
		var dialog = jQuery('div.treeview-create-folder-dialog');

		var item = getFileBrowserTreeViewSelectedItemPath('folder');

		dialog.find('input#relativeFolderHidden').val(item);
		dialog.find('span.relative-folder').html(item);

		dialog.dialog('open');

		return false;
	});

	controls.find('a.delete').click(function () {
		var dialog = jQuery('div.treeview-delete-item-dialog');

		var item = getFileBrowserTreeViewSelectedItemPath();

		dialog.find('input#itemHidden').val(item);
		dialog.find('p.item-tobe-deleted').find('strong').html(item);

		dialog.dialog('open');

		return false;
	});
};
/* ****************************************************************************
// Configura o diálogo para criação de uma nova pasta no tree view.
**************************************************************************** */
var configFileBrowserCreateFolderDialog = function () {

	var dialog = jQuery('div.treeview-create-folder-dialog');

	dialog.dialog({
		resizable: false,
		autoOpen: false,
		height: 360,
		width: 400,
		modal: true,
		title: 'Criar uma nova pasta',
		buttons: {
			'Criar nova pasta': function () {
				createFileBrowserNewFolder(dialog);
			},
			'Cancelar': function () {
				jQuery(this).dialog('close');
			}
		}
	});
};
/* ****************************************************************************
// Configura o diálogo para remover um item do tree view.
**************************************************************************** */
var configFileBrowserDeleteItemDialog = function () {

	var dialog = jQuery('div.treeview-delete-item-dialog');

	dialog.dialog({
		resizable: false,
		autoOpen: false,
		height: 200,
		width: 400,
		modal: true,
		title: 'Remover um item',
		buttons: {
			'Remover item': function () {
				deleteFileBrowserItem(dialog);
			},
			'Cancelar': function () {
				jQuery(this).dialog('close');
			}
		}
	});
};
/* ****************************************************************************
// Cria uma nova pasta no tree view (dentro da aplicação)
**************************************************************************** */
var createFileBrowserNewFolder = function (dialog) {

	var relativeFolder = dialog.find('input#relativeFolderHidden').val();
	var folder = dialog.find('input#folderText').val();

	jQuery.ajax({
		beforeSend: function (jqXHR, settings) {
		},
		cache: false,
		data: {
			relativeFolder: relativeFolder,
			folder: folder
		},
		dataType: 'html',
		error: function (jqXHR, textStatus, errorThrown) {
		},
		success: function (data, textStatus, jqXHR) {
			alert(data);
			renderFileBrowserTreeView();
			dialog.dialog('close');
		},
		type: 'POST',
		url: 'Handlers/CreateFileBrowserNewFolder.ashx'
	});
};
/* ****************************************************************************
// Cria uma nova pasta no tree view (dentro da aplicação)
**************************************************************************** */
var deleteFileBrowserItem = function (dialog) {

	var item = dialog.find('input#itemHidden').val();

	jQuery.ajax({
		beforeSend: function (jqXHR, settings) {
		},
		cache: false,
		data: { item: item },
		dataType: 'html',
		error: function (jqXHR, textStatus, errorThrown) {
		},
		success: function (data, textStatus, jqXHR) {
			alert(data);
			renderFileBrowserTreeView();
			dialog.dialog('close');
		},
		type: 'POST',
		url: 'Handlers/DeleteFileBrowserItem.ashx'
	});
};
/* ****************************************************************************
// Cria uma nova pasta no tree view (dentro da aplicação)
**************************************************************************** */
var getFileBrowserTreeViewSelectedItemPath = function (type) {

	var href = jQuery('ul.treeview')
					.find('span.selected')
					.find('a')
					.attr('href') || '#/';

	href = href.substring(1);

	return (href.length > 1 && type == 'folder' ? href.substring(0, href.lastIndexOf('/') + 1) : href);
};
/* ****************************************************************************
// Configura o Uploadify
**************************************************************************** */
var configUploadify = function () {
	jQuery('input#uploadFile').uploadify({
		uploader: 'static/media/jquery.uploadify.swf',
		script: 'handlers/uploadfiles.ashx',
		cancelImg: 'static/images/jquery.uploadify/cancel.png',
		multi: true,
		method: 'POST',
		buttonText: 'SELECIONAR',
		onAllComplete: function (e, data) {
			alert('Upload dos arquivos concluído com sucesso.');
			renderFileBrowserTreeView();
		}
	});
};
/* ****************************************************************************
// Dispara o evento de upload o Uploadify
**************************************************************************** */
var bindUploadify = function () {
	jQuery('input#sendFilesButton').click(function () {
		jQuery('input#uploadFile').uploadifyUpload();
	});
};
/* ****************************************************************************
// Configura a pasta de uploads
**************************************************************************** */
var configUploadifyFolder = function (folder) {
	jQuery('input#uploadFile').uploadifySettings('folder', folder);
};
/* ****************************************************************************
// Configura o botão de seleção do arquivo para o CKEditor
**************************************************************************** */
var bindCKEditorSelectItem = function () {
	jQuery('input#selectItemButton').click(function () {
		
		var path = getFileBrowserTreeViewSelectedItemPath();
		var ckeditorFuncNum = getUrlParam('CKEditorFuncNum');
		window.opener.CKEDITOR.tools.callFunction(ckeditorFuncNum, path);
		window.close();
	});
};
/* ****************************************************************************
// Retorna parâmetros da URL para a janela do CKEditor
**************************************************************************** */
var getUrlParam = function (paramName) {
	var pattern = '(?:[\?&]|&amp;)' + paramName + '=([^&]+)';
	var reParam = new RegExp(pattern, 'i');
	var match = window.location.search.match(reParam);

	return (match && match.length > 1) ? match[1] : '';
};