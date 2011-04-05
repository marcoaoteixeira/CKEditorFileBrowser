using System.Collections.Generic;

namespace Jam.CKEditorFileBrowser.Web.Core.Infrastructure
{
	public sealed class TreeViewNode
	{
		#region Private Fields

		private string _name;
		private string _path;
		private TreeViewNodeType _type;
		private TreeViewNode _parent;
		private List<TreeViewNode> _children;

		#endregion

		#region Public Properties

		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}
		public string Path
		{
			get { return this._path; }
			set { this._path = value; }
		}
		public TreeViewNodeType Type
		{
			get { return this._type; }
			set { this._type = value; }
		}
		public TreeViewNode Parent
		{
			get { return this._parent; }
			set { this._parent = value; }
		}
		public List<TreeViewNode> Children
		{
			get { return this._children; }
			set { this._children = value; }
		}

		#endregion

		#region Public Constructors

		public TreeViewNode()
			: this(string.Empty, string.Empty, TreeViewNodeType.General, null, new TreeViewNode[0]) { }

		public TreeViewNode(string name, string path, TreeViewNodeType type, TreeViewNode parent, TreeViewNode[] children)
		{
			this._name = name;
			this._path = path;
			this._type = type;
			this._parent = parent;
			this._children = new List<TreeViewNode>(children ?? new TreeViewNode[0]);
		}

		#endregion

		#region Public Override Methods

		public override string ToString()
		{
			return this.Path;
		}

		#endregion
	}
}