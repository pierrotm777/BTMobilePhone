//==========================================================================================
//
//		OpenNETCF.Windows.Forms.DSAParameters
//		Copyright (c) 2003, OpenNETCF.org
//
//		This library is free software; you can redistribute it and/or modify it under 
//		the terms of the OpenNETCF.org Shared Source License.
//
//		This library is distributed in the hope that it will be useful, but 
//		WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
//		FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
//		for more details.
//
//		You should have received a copy of the OpenNETCF.org Shared Source License 
//		along with this library; if not, email licensing@opennetcf.org to request a copy.
//
//		If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
//		email licensing@opennetcf.org.
//
//		For general enquiries, email enquiries@opennetcf.org or visit our website at:
//		http://www.opennetcf.org
//
//		!!! A HUGE thank-you goes out to Casey Chesnut for supplying this class library !!!
//      !!! You can contact Casey at http://www.brains-n-brawn.com                      !!!
//
//==========================================================================================
using System;

namespace OpenNETCF.Security.Cryptography
{
	public struct DSAParameters
	{
		/*
		//private , <X> is the only private piece
		<DSAKeyValue>
 			<P>l6uv6xTaRAOjN4dlEiHJzWwpz6z2kMi3QMNAyoyqtxkq9O5xEZ9vxTBr/QuFCMFITHCAN5GMbujrUWdQxsR6xCzCc5JWrcQ61QucIfIcTafUQiRVqEvyaT5D8K6EcB176UeFFfeyIEBbhHjpCiJUtuULWnXVlQ75rmnsppG6VZU=</P>
			<Q>0OZifYAcs6ul6dFResP9RJUuaic=</Q>
			<G>ZnAApN4T9kILnzKrLhCEiYWhCVsmDciki9IZuQ86h1gpF7MfggnEssuZpAjkBCnLAsysYfl9pKMpMfVq2iXSADfPeAdT1zkcnr26YDooorleIW0oqw8gzPf3ty6HM8FrRvdL862eYqbIyckGQbg5DRPK3gIRwjL6ILl96j5BhkA=</G>
			<Y>JNk2irZCMkvgPkErwgt84MmlWhVAjHhQ/c2MJCY9hw9r/3QkfReIaPcucg7N7f325idN/KUePJDFJiRDvQXm6YTuZQxys7xF+MbLHrq1vmiNusDxNHQBNBdqNt0dO1j4c9/3AGP1YaNj+aHHI/tYacoKTm8Updt6dso8CYFj/vc=</Y>
			<J>ud4NbWiWJmgHwWBmpfNfQVd+0YD0XhuwiVXKLtG+Ltb8g1cNzhbk7CNsaIEzlT7wlWsk7izCcWGBj4zYtrDLMldVMfwOO2H4fpSmTGFM6mb25pxkefH6EAIuS1OTgx2DwS5eHUW8h3ABE95M</J>
			<Seed>v6dv23hrDAD1Ixm8dnLI3nW1pdI=</Seed>
			<PgenCounter>HA==</PgenCounter>
			<X>p6zyw3trB+C+7BMJWyDqgFLjMvA=</X>
		</DSAKeyValue>
		*/

		public byte [] P; 
		public byte [] Q; 
		public byte [] G; 
		public byte [] Y; 
		public byte [] J; 
		public byte [] Seed; 
		public int Counter; //PgenCounter
		public byte [] X; //private
	} 
}
