﻿using System.Collections.Generic;
using CodeEditor.CodeParsing;
using CodeEditor.CodeParsing.WordTypes;
using CodeEditor.Enums;

namespace CodeEditor.Tests.Mocks {
    internal class MockDefinitionLoader : IDefinitionLoader {

        #region public methods

        public IEnumerable<string> GetWords(TextType type, SupportedLanguages language) =>
            "abstract,as,base,bool,break,byte,case,catch,char,checked,class,const,continue,decimal,default,delegate,do,double,else,enum,event,explicit,extern,false,finally,fixed,float,for,foreach,goto,if,implicit,in,int,interface,internal,is,lock,long,namespace,new,null,object,operator,out,override,params,private,protected,public,readonly,ref,return,sbyte,sealed,short,sizeof,stackalloc,static,string,struct,switch,this,throw,true,try,typeof,uint,ulong,unchecked,unsafe,ushort,using,virtual,void,volatile,while,add,alias,ascending,async,await,descending,dynamic,from,get,global,group,into,join,let,orderby,partial,remove,select,set,value,var,where,yield"
                .Split(',');

        #endregion

    }
}
